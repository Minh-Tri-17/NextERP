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
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public SupplierOrderDetailService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(SupplierOrderDetailModel request)
        {
            if (request.Id == Guid.Empty)
            {
                var supplierOrderDetail = new SupplierOrderDetail();
                DataHelper.MapAudit(request, supplierOrderDetail, _currentUser.UserName);

                await _context.SupplierOrderDetails.AddAsync(supplierOrderDetail);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var supplierOrderDetail = await _context.SupplierOrderDetails.FindAsync(request.Id);
                if (supplierOrderDetail == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, supplierOrderDetail, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listSupplierOrderDetailId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listSupplierOrderDetail = await _context.SupplierOrderDetails
                .Where(x => listSupplierOrderDetailId.Contains(x.Id))
                .ToListAsync();

            foreach (var supplierOrderDetail in listSupplierOrderDetail)
            {
                supplierOrderDetail.IsDelete = true;
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
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supplierOrderDetail == null)
                return new APIErrorResult<SupplierOrderDetailModel>(Messages.NotFoundGet);

            var supplierOrderDetailModel = DataHelper.Mapping<SupplierOrderDetail, SupplierOrderDetailModel>(supplierOrderDetail);

            return new APISuccessResult<SupplierOrderDetailModel>(Messages.GetResultSuccess, supplierOrderDetailModel);
        }

        public async Task<APIBaseResult<PagingResult<SupplierOrderDetailModel>>> GetPaging(Filter filter)
        {
            IQueryable<SupplierOrderDetail> query = _context.SupplierOrderDetails
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.SupplierOrderDetailCode)
                    && x.SupplierOrderDetailCode.ToLower().Contains(keyword));
            }

            var listSupplierOrderDetail = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            if (!listSupplierOrderDetail.Any())
                return new APIErrorResult<PagingResult<SupplierOrderDetailModel>>(Messages.NotFoundGetList);

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
    }
}
