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
    public class TrainingSessionService : ITrainingSessionService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public TrainingSessionService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(TrainingSessionModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var trainingSession = new TrainingSession();
                DataHelper.MapAudit(request, trainingSession, _currentUser.UserName);

                await _context.TrainingSessions.AddAsync(trainingSession);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var trainingSession = await _context.TrainingSessions.FindAsync(id);
                if (trainingSession == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, trainingSession, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listTrainingSessionId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listTrainingSession = await _context.TrainingSessions
                .Where(s => listTrainingSessionId.Contains(s.Id))
                .ToListAsync();

            foreach (var trainingSession in listTrainingSession)
            {
                trainingSession.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listTrainingSessionId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listTrainingSession = await _context.TrainingSessions
                .Where(s => listTrainingSessionId.Contains(s.Id))
                .ToListAsync();

            foreach (var trainingSession in listTrainingSession)
            {
                _context.TrainingSessions.Remove(trainingSession); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<TrainingSessionModel>> GetOne(Guid id)
        {
            var trainingSession = await _context.TrainingSessions
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (trainingSession == null)
                return new APIErrorResult<TrainingSessionModel>(Messages.NotFoundGet);

            var trainingSessionModel = DataHelper.Mapping<TrainingSession, TrainingSessionModel>(trainingSession);

            return new APISuccessResult<TrainingSessionModel>(Messages.GetResultSuccess, trainingSessionModel);
        }

        public async Task<APIBaseResult<PagingResult<TrainingSessionModel>>> GetPaging(TrainingSessionModel request)
        {
            IQueryable<TrainingSession> query = _context.TrainingSessions.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            Filter filter = new Filter()
            {
                KeyWord = request.TrainingSessionCode,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                IsDelete = DataHelper.GetBool(request.IsDelete)
            };

            query = query.ApplyCommonFilters(filter, s => s.TrainingSessionCode!, s => s.IsDelete, s => s.Id);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listTrainingSession = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listTrainingSessionModel = DataHelper.MappingList<TrainingSession, TrainingSessionModel>(listTrainingSession);
            var pageResult = new PagingResult<TrainingSessionModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listTrainingSessionModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listTrainingSessionModel
            };

            return new APISuccessResult<PagingResult<TrainingSessionModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listTrainingSessionModel = new List<TrainingSessionModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);
                if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
                    continue; // Bỏ qua hàng trống

                TrainingSessionModel trainingSessionModel = DataHelper.CopyImport<TrainingSessionModel>(headerRow, row);
                listTrainingSessionModel.Add(trainingSessionModel);
            }

            var listTrainingSession = new List<TrainingSession>();
            DataHelper.MapListAudit<TrainingSessionModel, TrainingSession>(listTrainingSessionModel, listTrainingSession, _currentUser.UserName);

            await _context.TrainingSessions.AddRangeAsync(listTrainingSession);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(TrainingSessionModel request)
        {
            var data = await GetPaging(request);
            var items = data?.Result?.Items ?? new List<TrainingSessionModel>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.TrainingSession);

            var listTrainingSession = DataHelper.MappingList<TrainingSessionModel, TrainingSession>(items);
            DataHelper.CopyExport(worksheet, listTrainingSession);

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
