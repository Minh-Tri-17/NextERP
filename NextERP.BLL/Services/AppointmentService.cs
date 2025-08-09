using ClosedXML.Excel;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.Util;
using NPOI.SS.Formula.Functions;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Service
{
    public class AppointmentService : IAppointmentService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public AppointmentService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(AppointmentModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var appointment = new Appointment();
                DataHelper.MapAudit(request, appointment, _currentUser.UserName);

                await _context.Appointments.AddAsync(appointment);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, appointment, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listAppointmentId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listAppointment = await _context.Appointments
                .Where(s => listAppointmentId.Contains(s.Id))
                .ToListAsync();

            foreach (var appointment in listAppointment)
            {
                appointment.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listAppointmentId = ids.Split(',')
                 .Select(id => DataHelper.GetGuid(id.Trim()))
                 .Where(guid => guid != Guid.Empty)
                 .ToList();

            var listAppointment = await _context.Appointments
                .Where(s => listAppointmentId.Contains(s.Id))
                .ToListAsync();

            foreach (var appointment in listAppointment)
            {
                _context.Appointments.Remove(appointment); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<AppointmentModel>> GetOne(Guid id)
        {
            var appointment = await _context.Appointments
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (appointment == null)
                return new APIErrorResult<AppointmentModel>(Messages.NotFoundGet);

            var appointmentModel = DataHelper.Mapping<Appointment, AppointmentModel>(appointment);

            return new APISuccessResult<AppointmentModel>(Messages.GetResultSuccess, appointmentModel);
        }

        public async Task<APIBaseResult<PagingResult<AppointmentModel>>> GetPaging(Filter filter)
        {
            IQueryable<Appointment> query = _context.Appointments.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            query = query.ApplyCommonFilters(filter, s => s.AppointmentCode!, s => s.IsDelete, s => s.Id);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listAppointment = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listAppointmentModel = DataHelper.MappingList<Appointment, AppointmentModel>(listAppointment);
            var pageResult = new PagingResult<AppointmentModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listAppointmentModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listAppointmentModel
            };

            return new APISuccessResult<PagingResult<AppointmentModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listAppointmentModel = new List<AppointmentModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);
                if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
                    continue; // Bỏ qua hàng trống

                AppointmentModel appointmentModel = DataHelper.CopyImport<AppointmentModel>(headerRow, row);
                listAppointmentModel.Add(appointmentModel);
            }

            var listAppointment = new List<Appointment>();
            DataHelper.MapListAudit<AppointmentModel, Appointment>(listAppointmentModel, listAppointment, _currentUser.UserName);

            await _context.Appointments.AddRangeAsync(listAppointment);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            var data = await GetPaging(filter);
            var items = data?.Result?.Items ?? new List<AppointmentModel>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.Appointment);

            var listAppointment = DataHelper.MappingList<AppointmentModel, Appointment>(items);
            DataHelper.CopyExport(worksheet, listAppointment);

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