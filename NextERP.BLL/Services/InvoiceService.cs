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
    public class InvoiceService : IInvoiceService
    {
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public InvoiceService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(InvoiceModel request)
        {
            if (request.Id == Guid.Empty)
            {
                var invoice = new Invoice();
                DataHelper.MapAudit(request, invoice, _currentUser.UserName);

                await _context.Invoices.AddAsync(invoice);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var invoice = await _context.Invoices.FindAsync(request.Id);
                if (invoice == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, invoice, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listInvoiceId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listInvoice = await _context.Invoices
                .Where(x => listInvoiceId.Contains(x.Id))
                .ToListAsync();

            foreach (var invoice in listInvoice)
            {
                invoice.IsDelete = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<InvoiceModel>> GetOne(Guid id)
        {
            var invoice = await _context.Invoices
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(x => x.Id == id);

            if (invoice == null)
                return new APIErrorResult<InvoiceModel>(Messages.NotFoundGet);

            var invoiceModel = DataHelper.Mapping<Invoice, InvoiceModel>(invoice);

            return new APISuccessResult<InvoiceModel>(Messages.GetResultSuccess, invoiceModel);
        }

        public async Task<APIBaseResult<PagingResult<InvoiceModel>>> GetPaging(Filter filter)
        {
            IQueryable<Invoice> query = _context.Invoices
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.InvoiceCode)
                    && x.InvoiceCode.ToLower().Contains(keyword));
            }

            var listInvoice = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            if (!listInvoice.Any())
                return new APIErrorResult<PagingResult<InvoiceModel>>(Messages.NotFoundGetList);

            var listInvoiceModel = DataHelper.MappingList<Invoice, InvoiceModel>(listInvoice);
            var pageResult = new PagingResult<InvoiceModel>()
            {
                TotalRecord = await query.CountAsync(),
                PageRecord = listInvoiceModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listInvoiceModel
            };

            return new APISuccessResult<PagingResult<InvoiceModel>>(Messages.GetListResultSuccess, pageResult);
        }
    }
}
