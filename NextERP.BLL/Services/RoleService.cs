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
    public class RoleService : IRoleService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public RoleService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(RoleModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var role = new Role();
                DataHelper.MapAudit(request, role, _currentUser.UserName);

                await _context.Roles.AddAsync(role);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, role, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listRoleId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listRole = await _context.Roles
                .Where(s => listRoleId.Contains(s.Id))
                .ToListAsync();

            foreach (var role in listRole)
            {
                role.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listRoleId = ids.Split(',')
                 .Select(id => DataHelper.GetGuid(id.Trim()))
                 .Where(guid => guid != Guid.Empty)
                 .ToList();

            var listRole = await _context.Roles
                .Where(s => listRoleId.Contains(s.Id))
                .ToListAsync();

            foreach (var role in listRole)
            {
                _context.Roles.Remove(role); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<RoleModel>> GetOne(Guid id)
        {
            var role = await _context.Roles
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (role == null)
                return new APIErrorResult<RoleModel>(Messages.NotFoundGet);

            var roleModel = DataHelper.Mapping<Role, RoleModel>(role);

            return new APISuccessResult<RoleModel>(Messages.GetResultSuccess, roleModel);
        }

        public async Task<APIBaseResult<PagingResult<RoleModel>>> GetPaging(RoleModel request)
        {
            IQueryable<Role> query = _context.Roles.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            Filter filter = new Filter()
            {
                KeyWord = request.RoleCode,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                IsDelete = DataHelper.GetBool(request.IsDelete)
            };

            query = query.ApplyCommonFilters(filter, s => s.RoleCode!, s => s.IsDelete, s => s.Id);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listRole = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listRoleModel = DataHelper.MappingList<Role, RoleModel>(listRole);
            var pageResult = new PagingResult<RoleModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listRoleModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listRoleModel
            };

            return new APISuccessResult<PagingResult<RoleModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listRoleModel = new List<RoleModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);
                if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
                    continue; // Bỏ qua hàng trống

                RoleModel roleModel = DataHelper.CopyImport<RoleModel>(headerRow, row);
                listRoleModel.Add(roleModel);
            }

            var listRole = new List<Role>();
            DataHelper.MapListAudit<RoleModel, Role>(listRoleModel, listRole, _currentUser.UserName);

            await _context.Roles.AddRangeAsync(listRole);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(RoleModel request)
        {
            var data = await GetPaging(request);
            var items = data?.Result?.Items ?? new List<RoleModel>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.Role);

            var listRole = DataHelper.MappingList<RoleModel, Role>(items);
            DataHelper.CopyExport(worksheet, listRole);

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
