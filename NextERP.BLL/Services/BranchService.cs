using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Service
{
    public class BranchService : IBranchService
    {
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public BranchService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(BranchModel request)
        {
            if (request.Id == Guid.Empty)
            {
                var branch = new Branch();
                DataHelper.MapAudit(request, branch, _currentUser.UserName);

                await _context.Branches.AddAsync(branch);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var branch = await _context.Branches.FindAsync(request.Id);
                if (branch == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, branch, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listBranchId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listBranch = await _context.Branches
                .Where(x => listBranchId.Contains(x.Id))
                .ToListAsync();

            foreach (var branch in listBranch)
            {
                branch.IsDelete = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<BranchModel>> GetOne(Guid id)
        {
            var branch = await _context.Branches
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(x => x.Id == id);

            if (branch == null)
                return new APIErrorResult<BranchModel>(Messages.NotFoundGet);

            var branchModel = DataHelper.Mapping<Branch, BranchModel>(branch);

            return new APISuccessResult<BranchModel>(Messages.GetResultSuccess, branchModel);
        }

        public async Task<APIBaseResult<PagingResult<BranchModel>>> GetPaging(Filter filter)
        {
            IQueryable<Branch> query = _context.Branches
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.BranchCode)
                    && x.BranchCode.ToLower().Contains(keyword));
            }

            var totalCount = await query.CountAsync();

            if (filter.AllowPaging)
            {
                query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            if (!string.IsNullOrEmpty(filter.Ids))
            {
                List<Guid> listBranchId = filter.Ids.Split(',')
                     .Select(id => DataHelper.GetGuid(id.Trim()))
                     .Where(guid => guid != Guid.Empty)
                     .ToList();

                query = query.Where(x => listBranchId.Contains(x.Id));
            }

            var listBranch = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .ToListAsync();

            if (!listBranch.Any())
                return new APIErrorResult<PagingResult<BranchModel>>(Messages.NotFoundGetList);

            var listBranchModel = DataHelper.MappingList<Branch, BranchModel>(listBranch);
            var pageResult = new PagingResult<BranchModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listBranchModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listBranchModel
            };

            return new APISuccessResult<PagingResult<BranchModel>>(Messages.GetListResultSuccess, pageResult);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            var stream = new MemoryStream();
            await fileImport.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);

            // Header data
            var headerRow = sheet.GetRow(0);

            var listBranchModel = new List<BranchModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);

                if (row != null)
                {
                    BranchModel branchModel = DataHelper.CopyImport<BranchModel>(headerRow, row);
                    listBranchModel.Add(branchModel);
                }
            }

            var listBranch = new List<Branch>();
            DataHelper.MapListAudit<BranchModel, Branch>(listBranchModel, listBranch, _currentUser.UserName);

            await _context.Branches.AddRangeAsync(listBranch);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            var data = await GetPaging(filter);
            var items = data?.Result?.Items;

            if (items == null || items.Count == 0)
                return new APIErrorResult<byte[]>(Messages.NotFoundGetList);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.Branch);

            var listBranch = DataHelper.MappingList<BranchModel, Branch>(items);
            DataHelper.CopyExport(worksheet, listBranch);

            var stream = new MemoryStream();
            workbook.SaveAs(stream); // Ghi nội dung của workbook(Excel) vào stream
            var bytes = stream.ToArray(); // Chuyển toàn bộ nội dung stream thành mảng byte
            if (bytes.Length > 0)
                return new APISuccessResult<byte[]>(Messages.ExportSuccess, bytes);

            return new APIErrorResult<byte[]>(Messages.ExportFailed);
        }
    }
}
