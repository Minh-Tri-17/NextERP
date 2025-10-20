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
    public class InvoiceDetailService : IInvoiceDetailService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public InvoiceDetailService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(InvoiceDetailModel request)
        {
            var product = await _context.Products.FirstOrDefaultAsync(s => s.Id == request.ProductId);
            if (product == null)
                return new APIErrorResult<bool>(Messages.NotFoundProduct);

            var invoiceDetailUpdate = await _context.InvoiceDetails.FirstOrDefaultAsync(s => s.ProductId == request.ProductId && !s.InvoiceId.HasValue);

            if (invoiceDetailUpdate != null)
            {
                invoiceDetailUpdate.Quantity += request.Quantity;
                invoiceDetailUpdate.UnitPrice = product.Price;
                invoiceDetailUpdate.TotalPrice = invoiceDetailUpdate.Quantity * invoiceDetailUpdate.UnitPrice;
            }
            else
            {
                var invoiceDetail = new InvoiceDetail();
                request.UnitPrice = product.Price;
                request.TotalPrice = request.Quantity * request.UnitPrice;
                DataHelper.MapAudit(request, invoiceDetail, _currentUser.UserName);

                await _context.InvoiceDetails.AddAsync(invoiceDetail);
            }

            product.QuantityInStock = product.QuantityInStock - request.Quantity;

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.CreateSuccess, true);

            return new APIErrorResult<bool>(Messages.CreateFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listInvoiceDetailId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listInvoiceDetail = await _context.InvoiceDetails
                .Where(s => listInvoiceDetailId.Contains(s.Id))
                .ToListAsync();

            var listProductId = listInvoiceDetail.Select(s => s.ProductId).Where(s => s.HasValue).Distinct().ToList();

            var listProduct = await _context.Products
                .Where(s => listProductId.Contains(s.Id))
                .ToListAsync();

            foreach (var invoiceDetail in listInvoiceDetail)
            {
                var product = listProduct.FirstOrDefault(s => s.Id == invoiceDetail.ProductId);
                if (product != null)
                {
                    product.QuantityInStock = product.QuantityInStock + invoiceDetail.Quantity;
                }

                _context.InvoiceDetails.Remove(invoiceDetail); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<PagingResult<InvoiceDetailModel>>> GetPaging(FilterModel filter)
        {
            IQueryable<InvoiceDetail> query = _context.InvoiceDetails.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            var isCart = filter.Filters.Where(s => s.FilterName == InvoiceModel.AttributeNames.InvoiceId)
                .Select(s => DataHelper.GetBool(s.FilterValue)).FirstOrDefault();

            if (isCart)
                query = query.Where(s => !s.InvoiceId.HasValue);
            else
                query = query.Where(s => s.InvoiceId.HasValue && s.InvoiceId == filter.IdMain);

            var listInvoiceDetail = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listInvoiceDetailModel = DataHelper.MappingList<InvoiceDetail, InvoiceDetailModel>(listInvoiceDetail);
            var pageResult = new PagingResult<InvoiceDetailModel>()
            {
                TotalRecord = await query.CountAsync(),
                PageRecord = listInvoiceDetailModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listInvoiceDetailModel
            };

            return new APISuccessResult<PagingResult<InvoiceDetailModel>>(Messages.GetListResultSuccess, pageResult);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
