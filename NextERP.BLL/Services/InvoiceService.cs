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
		#region Infrastructure

		private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
		private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

		public InvoiceService(NextErpContext context, ICurrentUserService currentUser)
		{
			_context = context;
			_currentUser = currentUser;
		}

		#endregion

		#region Default Operations

		public async Task<APIBaseResult<bool>> CreateOrEdit(InvoiceModel request)
		{
			#region Check null request and create variable

			var id = DataHelper.GetGuid(request.Id);

			#endregion

			if (id == Guid.Empty)
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
				var invoice = await _context.Invoices.FindAsync(id);
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
				.Where(s => listInvoiceId.Contains(s.Id))
				.ToListAsync();

			foreach (var invoice in listInvoice)
			{
				invoice.IsDelete = true; // Đánh dấu là đã xóa
			}

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

			return new APIErrorResult<bool>(Messages.DeleteFailed);
		}

		public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
		{
			List<Guid> listInvoiceId = ids.Split(',')
				 .Select(id => DataHelper.GetGuid(id.Trim()))
				 .Where(guid => guid != Guid.Empty)
				 .ToList();

			var listInvoice = await _context.Invoices
				.Where(s => listInvoiceId.Contains(s.Id))
				.ToListAsync();

			foreach (var invoice in listInvoice)
			{
				_context.Invoices.Remove(invoice); // Xóa vĩnh viễn
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
				.FirstOrDefaultAsync(s => s.Id == id);

			if (invoice == null)
				return new APIErrorResult<InvoiceModel>(Messages.NotFoundGet);

			var invoiceModel = DataHelper.Mapping<Invoice, InvoiceModel>(invoice);

			return new APISuccessResult<InvoiceModel>(Messages.GetResultSuccess, invoiceModel);
		}

		public async Task<APIBaseResult<PagingResult<InvoiceModel>>> GetPaging(FilterModel filter)
		{
			IQueryable<Invoice> query = _context.Invoices.AsNoTracking(); // Không theo dõi thay đổi của thực thể

			query = query.ApplyCommonFilters(filter);

			var totalCount = await query.CountAsync();

			query = query.ApplyPaging(filter);

			var listInvoice = await query
				.OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
				.ToListAsync();

			var listInvoiceModel = DataHelper.MappingList<Invoice, InvoiceModel>(listInvoice);
			var pageResult = new PagingResult<InvoiceModel>()
			{
				TotalRecord = totalCount,
				PageRecord = listInvoiceModel.Count(),
				PageIndex = filter.PageIndex,
				PageSize = filter.PageSize,
				Items = listInvoiceModel
			};

			return new APISuccessResult<PagingResult<InvoiceModel>>(Messages.GetListResultSuccess, pageResult);
		}

		#endregion

		#region Custom Operations

		#endregion
	}
}
