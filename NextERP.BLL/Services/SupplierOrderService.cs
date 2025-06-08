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
            if (request.Id == Guid.Empty)
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
                var supplierOrder = await _context.SupplierOrders.FindAsync(request.Id);
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
                .Where(x => listSupplierOrderId.Contains(x.Id))
                .ToListAsync();

            foreach (var supplierOrder in listSupplierOrder)
            {
                supplierOrder.IsDelete = true;
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
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supplierOrder == null)
                return new APIErrorResult<SupplierOrderModel>(Messages.NotFoundGet);

            var supplierOrderModel = DataHelper.Mapping<SupplierOrder, SupplierOrderModel>(supplierOrder);

            return new APISuccessResult<SupplierOrderModel>(Messages.GetResultSuccess, supplierOrderModel);
        }

        public async Task<APIBaseResult<PagingResult<SupplierOrderModel>>> GetPaging(Filter filter)
        {
            IQueryable<SupplierOrder> query = _context.SupplierOrders
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.SupplierOrderCode)
                    && x.SupplierOrderCode.ToLower().Contains(keyword));
            }

            var listSupplierOrder = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            if (!listSupplierOrder.Any())
                return new APIErrorResult<PagingResult<SupplierOrderModel>>(Messages.NotFoundGetList);

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
