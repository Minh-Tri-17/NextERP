using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.Util;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Service
{
    public class TemplateNotificationService : ITemplateNotificationService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public TemplateNotificationService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(TemplateNotificationModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var templateNotification = new TemplateNotification();
                DataHelper.MapAudit(request, templateNotification, _currentUser.UserName);

                await _context.TemplateNotifications.AddAsync(templateNotification);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var templateNotification = await _context.TemplateNotifications.FindAsync(id);
                if (templateNotification == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, templateNotification, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listTemplateNotificationId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listTemplateNotification = await _context.TemplateNotifications
                .Where(s => listTemplateNotificationId.Contains(s.Id))
                .ToListAsync();

            foreach (var templateNotification in listTemplateNotification)
            {
                templateNotification.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listTemplateNotificationId = ids.Split(',')
                 .Select(id => DataHelper.GetGuid(id.Trim()))
                 .Where(guid => guid != Guid.Empty)
                 .ToList();

            var listTemplateNotification = await _context.TemplateNotifications
                .Where(s => listTemplateNotificationId.Contains(s.Id))
                .ToListAsync();

            foreach (var templateNotification in listTemplateNotification)
            {
                _context.TemplateNotifications.Remove(templateNotification); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<TemplateNotificationModel>> GetOne(Guid id)
        {
            var templateNotification = await _context.TemplateNotifications
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (templateNotification == null)
                return new APIErrorResult<TemplateNotificationModel>(Messages.NotFoundGet);

            var templateNotificationModel = DataHelper.Mapping<TemplateNotification, TemplateNotificationModel>(templateNotification);

            return new APISuccessResult<TemplateNotificationModel>(Messages.GetResultSuccess, templateNotificationModel);
        }

        public async Task<APIBaseResult<PagingResult<TemplateNotificationModel>>> GetPaging(Filter filter)
        {
            IQueryable<TemplateNotification> query = _context.TemplateNotifications.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            query = query.ApplyCommonFilters(filter);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listTemplateNotification = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listTemplateNotificationModel = DataHelper.MappingList<TemplateNotification, TemplateNotificationModel>(listTemplateNotification);
            var pageResult = new PagingResult<TemplateNotificationModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listTemplateNotificationModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listTemplateNotificationModel
            };

            return new APISuccessResult<PagingResult<TemplateNotificationModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listTemplateNotificationModel = new List<TemplateNotificationModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);
                if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
                    continue; // Bỏ qua hàng trống

                TemplateNotificationModel templateNotificationModel = DataHelper.CopyImport<TemplateNotificationModel>(headerRow, row);
                listTemplateNotificationModel.Add(templateNotificationModel);
            }

            var listTemplateNotification = new List<TemplateNotification>();
            DataHelper.MapListAudit<TemplateNotificationModel, TemplateNotification>(listTemplateNotificationModel, listTemplateNotification, _currentUser.UserName);

            await _context.TemplateNotifications.AddRangeAsync(listTemplateNotification);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            var data = await GetPaging(filter);
            var items = data?.Result?.Items ?? new List<TemplateNotificationModel>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.TemplateNotification);

            var listTemplateNotification = DataHelper.MappingList<TemplateNotificationModel, TemplateNotification>(items);
            DataHelper.CopyExport(worksheet, listTemplateNotification);

            var stream = new MemoryStream();
            workbook.SaveAs(stream); // Ghi nội dung của workbook(Excel) vào stream
            var bytes = stream.ToArray(); // Chuyển toàn bộ nội dung stream thành mảng byte
            if (bytes.Length > 0)
                return new APISuccessResult<byte[]>(Messages.ExportSuccess, bytes);

            return new APIErrorResult<byte[]>(Messages.ExportFailed);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
