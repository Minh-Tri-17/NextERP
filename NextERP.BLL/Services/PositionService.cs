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
    public class PositionService : IPositionService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public PositionService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(PositionModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var position = new Position();
                DataHelper.MapAudit(request, position, _currentUser.UserName);

                await _context.Positions.AddAsync(position);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var position = await _context.Positions.FindAsync(id);
                if (position == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, position, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listPositionId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listPosition = await _context.Positions
                .Where(s => listPositionId.Contains(s.Id))
                .ToListAsync();

            foreach (var position in listPosition)
            {
                position.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listPositionId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listPosition = await _context.Positions
                .Where(s => listPositionId.Contains(s.Id))
                .ToListAsync();

            foreach (var position in listPosition)
            {
                _context.Positions.Remove(position); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<PositionModel>> GetOne(Guid id)
        {
            var position = await _context.Positions
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (position == null)
                return new APIErrorResult<PositionModel>(Messages.NotFoundGet);

            var positionModel = DataHelper.Mapping<Position, PositionModel>(position);

            return new APISuccessResult<PositionModel>(Messages.GetResultSuccess, positionModel);
        }

        public async Task<APIBaseResult<PagingResult<PositionModel>>> GetPaging(PositionModel request)
        {
            IQueryable<Position> query = _context.Positions.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            Filter filter = new Filter()
            {
                KeyWord = request.PositionCode,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                IsDelete = DataHelper.GetBool(request.IsDelete)
            };

            query = query.ApplyCommonFilters(filter, s => s.PositionCode!, s => s.IsDelete, s => s.Id);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listPosition = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listPositionModel = DataHelper.MappingList<Position, PositionModel>(listPosition);
            var pageResult = new PagingResult<PositionModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listPositionModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listPositionModel
            };

            return new APISuccessResult<PagingResult<PositionModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listPositionModel = new List<PositionModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);
                if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
                    continue; // Bỏ qua hàng trống

                PositionModel positionModel = DataHelper.CopyImport<PositionModel>(headerRow, row);
                listPositionModel.Add(positionModel);
            }

            var listPosition = new List<Position>();
            DataHelper.MapListAudit<PositionModel, Position>(listPositionModel, listPosition, _currentUser.UserName);

            await _context.Positions.AddRangeAsync(listPosition);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(PositionModel request)
        {
            var data = await GetPaging(request);
            var items = data?.Result?.Items ?? new List<PositionModel>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.Position);

            var listPosition = DataHelper.MappingList<PositionModel, Position>(items);
            DataHelper.CopyExport(worksheet, listPosition);

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
