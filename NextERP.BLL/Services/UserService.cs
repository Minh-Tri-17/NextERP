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
    public class UserService : IUserService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại
        private readonly IAccountService _accountService;

        public UserService(NextErpContext context, ICurrentUserService currentUser, IAccountService accountService)
        {
            _context = context;
            _currentUser = currentUser;
            _accountService = accountService;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(UserModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);
            var employeeId = DataHelper.GetGuid(request.EmployeeId);
            var groupRole = DataHelper.GetString(request.GroupRole);
            var password = DataHelper.GetString(request.Password);
            var passwordHashed = PasswordHasher.HashPassword(password);

            #endregion

            var listRole = await _context.Roles.Where(s => !string.IsNullOrEmpty(s.GroupRole)
                    && s.GroupRole.Contains(groupRole)).Select(s => s.RoleName).ToListAsync();
            if (listRole == null || listRole.Count() == 0)
                return new APIErrorResult<bool>(Messages.RoleNotExist);

            var validationResult = _accountService.ValidatePassword(password);
            if (validationResult.Any())
            {
                var error = string.Join(", ", validationResult.Select(e =>
                    e.Description != null && e.Description.Length > 0
                    ? $"{e.Code}|{string.Join("|", e.Description)}" : e.Code));

                return new APIErrorResult<bool>(error);
            }

            request.PasswordHash = passwordHashed; // Mã hóa mật khẩu trước khi lưu
            request.RoleIds = string.Join(";", listRole);

            if (id == Guid.Empty)
            {
                var user = new User();
                DataHelper.MapAudit(request, user, _currentUser.UserName);

                var employee = await _context.Employees.FindAsync(employeeId);
                user.PhoneNumber = employee?.PhoneNumber;
                user.Mail = employee?.Email;

                await _context.Users.AddAsync(user);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, user, _currentUser.UserName);

                var employee = await _context.Employees.FindAsync(employeeId);
                user.PhoneNumber = employee?.PhoneNumber;
                user.Mail = employee?.Email;

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listUserId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listUser = await _context.Users
                .Where(s => listUserId.Contains(s.Id))
                .ToListAsync();

            foreach (var user in listUser)
            {
                user.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listUserId = ids.Split(',')
                 .Select(id => DataHelper.GetGuid(id.Trim()))
                 .Where(guid => guid != Guid.Empty)
                 .ToList();

            var listUser = await _context.Users
                .Where(s => listUserId.Contains(s.Id))
                .ToListAsync();

            foreach (var user in listUser)
            {
                _context.Users.Remove(user); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<UserModel>> GetOne(Guid id)
        {
            var user = await _context.Users
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id && s.Id != Guid.Empty);

            if (user == null)
                return new APIErrorResult<UserModel>(Messages.NotFoundGet);

            var userModel = DataHelper.Mapping<User, UserModel>(user);

            return new APISuccessResult<UserModel>(Messages.GetResultSuccess, userModel);
        }

        public async Task<APIBaseResult<PagingResult<UserModel>>> GetPaging(FilterModel filter)
        {
            IQueryable<User> query = _context.Users.AsNoTracking().Where(s => s.Id != Guid.Empty); // Không theo dõi thay đổi của thực thể

            query = query.ApplyCommonFilters(filter);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listUser = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listUserModel = DataHelper.MappingList<User, UserModel>(listUser);

            foreach (var item in listUserModel)
            {
                var employee = await _context.Employees.FindAsync(item.EmployeeId);
                item.EmployeeName = DataHelper.GetString(employee?.FullName);
                item.PhoneNumber = DataHelper.GetString(employee?.PhoneNumber);
            }

            var pageResult = new PagingResult<UserModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listUserModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listUserModel
            };

            return new APISuccessResult<PagingResult<UserModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listUserModel = new List<UserModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);
                if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
                    continue; // Bỏ qua hàng trống

                UserModel userModel = DataHelper.CopyImport<UserModel>(headerRow, row);
                listUserModel.Add(userModel);
            }

            var listUser = new List<User>();
            DataHelper.MapListAudit<UserModel, User>(listUserModel, listUser, _currentUser.UserName);

            await _context.Users.AddRangeAsync(listUser);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
        {
            var data = await GetPaging(filter);
            var items = data?.Result?.Items ?? new List<UserModel>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.User);

            var listUser = DataHelper.MappingList<UserModel, User>(items);
            DataHelper.CopyExport(worksheet, listUser);

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
