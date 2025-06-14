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
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public InvoiceDetailService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(InvoiceDetailModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var invoiceDetail = new InvoiceDetail();
                DataHelper.MapAudit(request, invoiceDetail, _currentUser.UserName);

                await _context.InvoiceDetails.AddAsync(invoiceDetail);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var invoiceDetail = await _context.InvoiceDetails.FindAsync(id);
                if (invoiceDetail == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, invoiceDetail, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listInvoiceDetailId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listInvoiceDetail = await _context.InvoiceDetails
                .Where(s => listInvoiceDetailId.Contains(s.Id))
                .ToListAsync();

            foreach (var invoiceDetail in listInvoiceDetail)
            {
                invoiceDetail.IsDelete = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<InvoiceDetailModel>> GetOne(Guid id)
        {
            var invoiceDetail = await _context.InvoiceDetails
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (invoiceDetail == null)
                return new APIErrorResult<InvoiceDetailModel>(Messages.NotFoundGet);

            var invoiceDetailModel = DataHelper.Mapping<InvoiceDetail, InvoiceDetailModel>(invoiceDetail);

            return new APISuccessResult<InvoiceDetailModel>(Messages.GetResultSuccess, invoiceDetailModel);
        }

        public async Task<APIBaseResult<PagingResult<InvoiceDetailModel>>> GetPaging(Filter filter)
        {
            IQueryable<InvoiceDetail> query = _context.InvoiceDetails
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(s => s.IsDelete != true);

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
    }
}
