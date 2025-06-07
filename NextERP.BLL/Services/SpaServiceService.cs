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
    public class SpaServiceService : ISpaServiceService
    {
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public SpaServiceService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(Guid id, SpaServiceModel request)
        {
            if (id == Guid.Empty)
            {
                var spaService = new SpaService();
                DataHelper.MapAudit(request, spaService, _currentUser.UserName);

                await _context.SpaServices.AddAsync(spaService);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var spaService = await _context.SpaServices.FindAsync(id);
                if (spaService == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, spaService, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listSpaServiceId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listSpaService = await _context.SpaServices
                .Where(x => listSpaServiceId.Contains(x.Id))
                .ToListAsync();

            foreach (var spaService in listSpaService)
            {
                spaService.IsDelete = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<SpaServiceModel>> GetOne(Guid id)
        {
            var spaService = await _context.SpaServices
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(x => x.Id == id);

            if (spaService == null)
                return new APIErrorResult<SpaServiceModel>(Messages.NotFoundGet);

            var spaServiceModel = DataHelper.Mapping<SpaService, SpaServiceModel>(spaService);

            return new APISuccessResult<SpaServiceModel>(Messages.GetResultSuccess, spaServiceModel);
        }

        public async Task<APIBaseResult<PagingResult<SpaServiceModel>>> GetPaging(Filter filter)
        {
            IQueryable<SpaService> query = _context.SpaServices
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.SpaServiceCode)
                    && x.SpaServiceCode.ToLower().Contains(keyword));
            }

            var totalCount = await query.CountAsync();

            if (filter.AllowPaging)
            {
                query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            if (!string.IsNullOrEmpty(filter.Ids))
            {
                List<Guid> listSpaServiceId = filter.Ids.Split(',')
                     .Select(id => DataHelper.GetGuid(id.Trim()))
                     .Where(guid => guid != Guid.Empty)
                     .ToList();

                query = query.Where(x => listSpaServiceId.Contains(x.Id));
            }

            var listSpaService = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .ToListAsync();

            if (!listSpaService.Any())
                return new APIErrorResult<PagingResult<SpaServiceModel>>(Messages.NotFoundGetList);

            var listSpaServiceModel = DataHelper.MappingList<SpaService, SpaServiceModel>(listSpaService);
            var pageResult = new PagingResult<SpaServiceModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listSpaServiceModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listSpaServiceModel
            };

            return new APISuccessResult<PagingResult<SpaServiceModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listSpaServiceModel = new List<SpaServiceModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);

                if (row != null)
                {
                    SpaServiceModel spaServiceModel = DataHelper.CopyImport<SpaServiceModel>(headerRow, row);
                    listSpaServiceModel.Add(spaServiceModel);
                }
            }

            var listSpaService = new List<SpaService>();
            DataHelper.MapListAudit<SpaServiceModel, SpaService>(listSpaServiceModel, listSpaService, _currentUser.UserName);

            await _context.SpaServices.AddRangeAsync(listSpaService);

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
            var worksheet = workbook.Worksheets.Add(TableName.SpaService);

            var listSpaService = DataHelper.MappingList<SpaServiceModel, SpaService>(items);
            DataHelper.CopyExport(worksheet, listSpaService);

            var stream = new MemoryStream();
            workbook.SaveAs(stream); // Ghi nội dung của workbook(Excel) vào stream
            var bytes = stream.ToArray(); // Chuyển toàn bộ nội dung stream thành mảng byte
            if (bytes.Length > 0)
                return new APISuccessResult<byte[]>(Messages.ExportSuccess, bytes);

            return new APIErrorResult<byte[]>(Messages.ExportFailed);
        }
    }
}
