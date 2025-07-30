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
    public class SupplierOrderService : ISupplierOrderService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public SupplierOrderService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listSupplierOrderId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listSupplierOrder = await _context.SupplierOrders
                .Where(s => listSupplierOrderId.Contains(s.Id))
                .ToListAsync();

            foreach (var supplierOrder in listSupplierOrder)
            {
                supplierOrder.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listSupplierOrderId = ids.Split(',')
                 .Select(id => DataHelper.GetGuid(id.Trim()))
                 .Where(guid => guid != Guid.Empty)
                 .ToList();

            var listSupplierOrder = await _context.SupplierOrders
                .Where(s => listSupplierOrderId.Contains(s.Id))
                .ToListAsync();

            foreach (var supplierOrder in listSupplierOrder)
            {
                _context.SupplierOrders.Remove(supplierOrder); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<SupplierOrderModel>> GetOne(Guid id)
        {
            var supplierOrder = await _context.SupplierOrders
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplierOrder == null)
                return new APIErrorResult<SupplierOrderModel>(Messages.NotFoundGet);

            var supplierOrderModel = DataHelper.Mapping<SupplierOrder, SupplierOrderModel>(supplierOrder);

            return new APISuccessResult<SupplierOrderModel>(Messages.GetResultSuccess, supplierOrderModel);
        }

        public async Task<APIBaseResult<PagingResult<SupplierOrderModel>>> GetPaging(Filter filter)
        {
            IQueryable<SupplierOrder> query = _context.SupplierOrders.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            query = query.ApplyCommonFilters(filter, s => s.SupplierOrderCode!, s => s.IsDelete, s => s.Id);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listSupplierOrder = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listSupplierOrderModel = DataHelper.MappingList<SupplierOrder, SupplierOrderModel>(listSupplierOrder);
            var pageResult = new PagingResult<SupplierOrderModel>()
            {
                TotalRecord = await query.CountAsync(),
                PageRecord = listSupplierOrderModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listSupplierOrderModel
            };

            return new APISuccessResult<PagingResult<SupplierOrderModel>>(Messages.GetListResultSuccess, pageResult);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            var stream = new MemoryStream();
            await fileImport.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);

            var listSupplierOrderModel = new List<SupplierOrderModel>();
            var listSupplierOrderDetailModel = new List<SupplierOrderDetailModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);
                if (row == null) continue;

                var model = DataHelper.CopyImport<SupplierOrderModel, SupplierOrderDetailModel>(row);
                model.Item2.SupplierOrderId = model.Item1.Id;

                listSupplierOrderModel.Add(model.Item1);
                listSupplierOrderDetailModel.Add(model.Item2);
            }

            var listSupplierOrder = new List<SupplierOrder>();
            DataHelper.MapListAudit<SupplierOrderModel, SupplierOrder>(listSupplierOrderModel, listSupplierOrder, _currentUser.UserName);
            await _context.SupplierOrders.AddRangeAsync(listSupplierOrder);

            var listSupplierOrderDetail = new List<SupplierOrderDetail>();
            DataHelper.MapListAudit<SupplierOrderDetailModel, SupplierOrderDetail>(listSupplierOrderDetailModel, listSupplierOrderDetail, _currentUser.UserName);
            await _context.SupplierOrderDetails.AddRangeAsync(listSupplierOrderDetail);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            var data = await GetPaging(filter);
            var items = data?.Result?.Items ?? new List<SupplierOrderModel>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.SupplierOrder);

            var listSupplier = DataHelper.MappingList<SupplierOrderModel, SupplierOrder>(items);
            DataHelper.CopyExport(worksheet, listSupplier);

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
