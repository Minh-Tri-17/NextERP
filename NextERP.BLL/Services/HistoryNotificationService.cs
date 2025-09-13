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
    public class HistoryNotificationService : IHistoryNotificationService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public HistoryNotificationService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(HistoryNotificationModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var historyNotification = new HistoryNotification();
                DataHelper.MapAudit(request, historyNotification, _currentUser.UserName);

                await _context.HistoryNotifications.AddAsync(historyNotification);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var historyNotification = await _context.HistoryNotifications.FindAsync(id);
                if (historyNotification == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, historyNotification, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listHistoryNotificationId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listHistoryNotification = await _context.HistoryNotifications
                .Where(s => listHistoryNotificationId.Contains(s.Id))
                .ToListAsync();

            foreach (var historyNotification in listHistoryNotification)
            {
                historyNotification.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listHistoryNotificationId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listHistoryNotification = await _context.HistoryNotifications
                .Where(s => listHistoryNotificationId.Contains(s.Id))
                .ToListAsync();

            foreach (var historyNotification in listHistoryNotification)
            {
                _context.HistoryNotifications.Remove(historyNotification); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<HistoryNotificationModel>> GetOne(Guid id)
        {
            var historyNotification = await _context.HistoryNotifications
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (historyNotification == null)
                return new APIErrorResult<HistoryNotificationModel>(Messages.NotFoundGet);

            var historyNotificationModel = DataHelper.Mapping<HistoryNotification, HistoryNotificationModel>(historyNotification);

            return new APISuccessResult<HistoryNotificationModel>(Messages.GetResultSuccess, historyNotificationModel);
        }

        public async Task<APIBaseResult<PagingResult<HistoryNotificationModel>>> GetPaging(Filter filter)
        {
            IQueryable<HistoryNotification> query = _context.HistoryNotifications.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            query = query.ApplyCommonFilters(filter);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listHistoryNotification = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listHistoryNotificationModel = DataHelper.MappingList<HistoryNotification, HistoryNotificationModel>(listHistoryNotification);
            var pageResult = new PagingResult<HistoryNotificationModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listHistoryNotificationModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listHistoryNotificationModel
            };

            return new APISuccessResult<PagingResult<HistoryNotificationModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listHistoryNotificationModel = new List<HistoryNotificationModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);
                if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
                    continue; // Bỏ qua hàng trống

                HistoryNotificationModel historyNotificationModel = DataHelper.CopyImport<HistoryNotificationModel>(headerRow, row);
                listHistoryNotificationModel.Add(historyNotificationModel);
            }

            var listHistoryNotification = new List<HistoryNotification>();
            DataHelper.MapListAudit<HistoryNotificationModel, HistoryNotification>(listHistoryNotificationModel, listHistoryNotification, _currentUser.UserName);

            await _context.HistoryNotifications.AddRangeAsync(listHistoryNotification);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            var data = await GetPaging(filter);
            var items = data?.Result?.Items ?? new List<HistoryNotificationModel>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.HistoryNotification);

            var listHistoryNotification = DataHelper.MappingList<HistoryNotificationModel, HistoryNotification>(items);
            DataHelper.CopyExport(worksheet, listHistoryNotification);

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
