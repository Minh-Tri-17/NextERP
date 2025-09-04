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
    public class SupplierOrderDetailService : ISupplierOrderDetailService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public SupplierOrderDetailService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listSupplierOrderDetailId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listSupplierOrderDetail = await _context.SupplierOrderDetails
                .Where(s => listSupplierOrderDetailId.Contains(s.Id))
                .ToListAsync();

            foreach (var supplierOrderDetail in listSupplierOrderDetail)
            {
                _context.SupplierOrderDetails.Remove(supplierOrderDetail); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<SupplierOrderDetailModel>> GetOne(Guid id)
        {
            var supplierOrderDetail = await _context.SupplierOrderDetails
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplierOrderDetail == null)
                return new APIErrorResult<SupplierOrderDetailModel>(Messages.NotFoundGet);

            var supplierOrderDetailModel = DataHelper.Mapping<SupplierOrderDetail, SupplierOrderDetailModel>(supplierOrderDetail);

            return new APISuccessResult<SupplierOrderDetailModel>(Messages.GetResultSuccess, supplierOrderDetailModel);
        }

        public async Task<APIBaseResult<PagingResult<SupplierOrderDetailModel>>> GetPaging(SupplierOrderDetailModel request)
        {
            Filter filter = new Filter()
            {
                KeyWord = request.SupplierOrderDetailCode,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                IdMain = request.SupplierOrderId
            };

            IQueryable<SupplierOrderDetail> query = _context.SupplierOrderDetails.AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(s => s.SupplierOrderId == filter.IdMain);

            var listSupplierOrderDetail = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listSupplierOrderDetailModel = DataHelper.MappingList<SupplierOrderDetail, SupplierOrderDetailModel>(listSupplierOrderDetail);
            var pageResult = new PagingResult<SupplierOrderDetailModel>()
            {
                TotalRecord = await query.CountAsync(),
                PageRecord = listSupplierOrderDetailModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listSupplierOrderDetailModel
            };

            return new APISuccessResult<PagingResult<SupplierOrderDetailModel>>(Messages.GetListResultSuccess, pageResult);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
