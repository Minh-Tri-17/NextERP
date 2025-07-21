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

namespace NextERP.BLL.Service
{
    public class AttendanceService : IAttendanceService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public AttendanceService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(AttendanceModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var attendance = new Attendance();
                DataHelper.MapAudit(request, attendance, _currentUser.UserName);

                await _context.Attendances.AddAsync(attendance);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var attendance = await _context.Attendances.FindAsync(id);
                if (attendance == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, attendance, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listAttendanceId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listAttendance = await _context.Attendances
                .Where(s => listAttendanceId.Contains(s.Id))
                .ToListAsync();

            foreach (var attendance in listAttendance)
            {
                attendance.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listAttendanceId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listAttendance = await _context.Attendances
                .Where(s => listAttendanceId.Contains(s.Id))
                .ToListAsync();

            foreach (var attendance in listAttendance)
            {
                _context.Attendances.Remove(attendance); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<AttendanceModel>> GetOne(Guid id)
        {
            var attendance = await _context.Attendances
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (attendance == null)
                return new APIErrorResult<AttendanceModel>(Messages.NotFoundGet);

            var attendanceModel = DataHelper.Mapping<Attendance, AttendanceModel>(attendance);

            return new APISuccessResult<AttendanceModel>(Messages.GetResultSuccess, attendanceModel);
        }

        public async Task<APIBaseResult<PagingResult<AttendanceModel>>> GetPaging(Filter filter)
        {
            IQueryable<Attendance> query = _context.Attendances.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            query = query.ApplyCommonFilters(filter, s => s.AttendanceCode!, s => s.IsDelete, s => s.Id);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listAttendance = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listAttendanceModel = DataHelper.MappingList<Attendance, AttendanceModel>(listAttendance);
            var pageResult = new PagingResult<AttendanceModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listAttendanceModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listAttendanceModel
            };

            return new APISuccessResult<PagingResult<AttendanceModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listAttendanceModel = new List<AttendanceModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);

                if (row != null)
                {
                    AttendanceModel attendanceModel = DataHelper.CopyImport<AttendanceModel>(headerRow, row);
                    listAttendanceModel.Add(attendanceModel);
                }
            }

            var listAttendance = new List<Attendance>();
            DataHelper.MapListAudit<AttendanceModel, Attendance>(listAttendanceModel, listAttendance, _currentUser.UserName);

            await _context.Attendances.AddRangeAsync(listAttendance);

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
            var worksheet = workbook.Worksheets.Add(TableName.Appointment);

            var listAttendance = DataHelper.MappingList<AttendanceModel, Attendance>(items);
            DataHelper.CopyExport(worksheet, listAttendance);

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
