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
    public class AttendanceService : IAttendanceService
    {
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public AttendanceService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(AttendanceModel request)
        {
            if (request.Id == Guid.Empty)
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
                var attendance = await _context.Attendances.FindAsync(request.Id);
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
                .Where(x => listAttendanceId.Contains(x.Id))
                .ToListAsync();

            foreach (var attendance in listAttendance)
            {
                attendance.IsDelete = true;
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
                .FirstOrDefaultAsync(x => x.Id == id);

            if (attendance == null)
                return new APIErrorResult<AttendanceModel>(Messages.NotFoundGet);

            var attendanceModel = DataHelper.Mapping<Attendance, AttendanceModel>(attendance);

            return new APISuccessResult<AttendanceModel>(Messages.GetResultSuccess, attendanceModel);
        }

        public async Task<APIBaseResult<PagingResult<AttendanceModel>>> GetPaging(Filter filter)
        {
            IQueryable<Attendance> query = _context.Attendances
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.AttendanceCode)
                    && x.AttendanceCode.ToLower().Contains(keyword));
            }

            var totalCount = await query.CountAsync();

            if (filter.AllowPaging)
            {
                query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            if (!string.IsNullOrEmpty(filter.Ids))
            {
                List<Guid> listAttendanceId = filter.Ids.Split(',')
                     .Select(id => DataHelper.GetGuid(id.Trim()))
                     .Where(guid => guid != Guid.Empty)
                     .ToList();

                query = query.Where(x => listAttendanceId.Contains(x.Id));
            }

            var listAttendance = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .ToListAsync();

            if (!listAttendance.Any())
                return new APIErrorResult<PagingResult<AttendanceModel>>(Messages.NotFoundGetList);

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
    }
}
