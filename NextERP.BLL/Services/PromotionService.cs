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
    public class PromotionService : IPromotionService
    {
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public PromotionService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(PromotionModel request)
        {
            if (request.Id == Guid.Empty)
            {
                var promotion = new Promotion();
                DataHelper.MapAudit(request, promotion, _currentUser.UserName);

                await _context.Promotions.AddAsync(promotion);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var promotion = await _context.Promotions.FindAsync(request.Id);
                if (promotion == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, promotion, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listPromotionId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listPromotion = await _context.Promotions
                .Where(x => listPromotionId.Contains(x.Id))
                .ToListAsync();

            foreach (var promotion in listPromotion)
            {
                promotion.IsDelete = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<PromotionModel>> GetOne(Guid id)
        {
            var promotion = await _context.Promotions
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(x => x.Id == id);

            if (promotion == null)
                return new APIErrorResult<PromotionModel>(Messages.NotFoundGet);

            var promotionModel = DataHelper.Mapping<Promotion, PromotionModel>(promotion);

            return new APISuccessResult<PromotionModel>(Messages.GetResultSuccess, promotionModel);
        }

        public async Task<APIBaseResult<PagingResult<PromotionModel>>> GetPaging(Filter filter)
        {
            IQueryable<Promotion> query = _context.Promotions
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.PromotionCode)
                    && x.PromotionCode.ToLower().Contains(keyword));
            }

            var totalCount = await query.CountAsync();

            if (filter.AllowPaging)
            {
                query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            if (!string.IsNullOrEmpty(filter.Ids))
            {
                List<Guid> listPromotionId = filter.Ids.Split(',')
                     .Select(id => DataHelper.GetGuid(id.Trim()))
                     .Where(guid => guid != Guid.Empty)
                     .ToList();

                query = query.Where(x => listPromotionId.Contains(x.Id));
            }

            var listPromotion = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .ToListAsync();

            if (!listPromotion.Any())
                return new APIErrorResult<PagingResult<PromotionModel>>(Messages.NotFoundGetList);

            var listPromotionModel = DataHelper.MappingList<Promotion, PromotionModel>(listPromotion);
            var pageResult = new PagingResult<PromotionModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listPromotionModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listPromotionModel
            };

            return new APISuccessResult<PagingResult<PromotionModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listPromotionModel = new List<PromotionModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);

                if (row != null)
                {
                    PromotionModel promotionModel = DataHelper.CopyImport<PromotionModel>(headerRow, row);
                    listPromotionModel.Add(promotionModel);
                }
            }

            var listPromotion = new List<Promotion>();
            DataHelper.MapListAudit<PromotionModel, Promotion>(listPromotionModel, listPromotion, _currentUser.UserName);

            await _context.Promotions.AddRangeAsync(listPromotion);

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
            var worksheet = workbook.Worksheets.Add(TableName.Promotion);

            var listPromotion = DataHelper.MappingList<PromotionModel, Promotion>(items);
            DataHelper.CopyExport(worksheet, listPromotion);

            var stream = new MemoryStream();
            workbook.SaveAs(stream); // Ghi nội dung của workbook(Excel) vào stream
            var bytes = stream.ToArray(); // Chuyển toàn bộ nội dung stream thành mảng byte
            if (bytes.Length > 0)
                return new APISuccessResult<byte[]>(Messages.ExportSuccess, bytes);

            return new APIErrorResult<byte[]>(Messages.ExportFailed);
        }
    }
}
