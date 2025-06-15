using Microsoft.EntityFrameworkCore;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Service
{
    public class SupplierOrderService : ISupplierOrderService
    {
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public SupplierOrderService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(SupplierOrderModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var supplierOrder = new SupplierOrder();
                DataHelper.MapAudit(request, supplierOrder, _currentUser.UserName);

                await _context.SupplierOrders.AddAsync(supplierOrder);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var supplierOrder = await _context.SupplierOrders.FindAsync(id);
                if (supplierOrder == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, supplierOrder, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

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
    }
}
