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
	public class SpaServiceCategoryService : ISpaServiceCategoryService
	{
		#region Infrastructure

		private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
		private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

		public SpaServiceCategoryService(NextErpContext context, ICurrentUserService currentUser)
		{
			_context = context;
			_currentUser = currentUser;
		}

		#endregion

		#region Default Operations

		public async Task<APIBaseResult<bool>> CreateOrEdit(SpaServiceCategoryModel request)
		{
			#region Check null request and create variable

			var id = DataHelper.GetGuid(request.Id);

			#endregion

			if (id == Guid.Empty)
			{
				var appointment = new SpaServiceCategory();
				DataHelper.MapAudit(request, appointment, _currentUser.UserName);

				await _context.SpaServiceCategories.AddAsync(appointment);

				var result = await _context.SaveChangesAsync();
				if (result > 0)
					return new APISuccessResult<bool>(Messages.CreateSuccess, true);

				return new APIErrorResult<bool>(Messages.CreateFailed);
			}
			else
			{
				var appointment = await _context.SpaServiceCategories.FindAsync(id);
				if (appointment == null)
					return new APIErrorResult<bool>(Messages.NotFoundUpdate);

				DataHelper.MapAudit(request, appointment, _currentUser.UserName);

				var result = await _context.SaveChangesAsync();
				if (result > 0)
					return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

				return new APIErrorResult<bool>(Messages.UpdateFailed);
			}
		}

		public async Task<APIBaseResult<bool>> Delete(string ids)
		{
			List<Guid> listSpaServiceCategoryId = ids.Split(',')
				.Select(id => DataHelper.GetGuid(id.Trim()))
				.Where(guid => guid != Guid.Empty)
				.ToList();

			var listSpaServiceCategory = await _context.SpaServiceCategories
				.Where(s => listSpaServiceCategoryId.Contains(s.Id))
				.ToListAsync();

			foreach (var appointment in listSpaServiceCategory)
			{
				appointment.IsDelete = true; // Đánh dấu là đã xóa
			}

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

			return new APIErrorResult<bool>(Messages.DeleteFailed);
		}

		public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
		{
			List<Guid> listSpaServiceCategoryId = ids.Split(',')
				.Select(id => DataHelper.GetGuid(id.Trim()))
				.Where(guid => guid != Guid.Empty)
				.ToList();

			var listSpaServiceCategory = await _context.SpaServiceCategories
				.Where(s => listSpaServiceCategoryId.Contains(s.Id))
				.ToListAsync();

			foreach (var appointment in listSpaServiceCategory)
			{
				_context.SpaServiceCategories.Remove(appointment); // Xóa vĩnh viễn
			}

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

			return new APIErrorResult<bool>(Messages.DeleteFailed);
		}

		public async Task<APIBaseResult<SpaServiceCategoryModel>> GetOne(Guid id)
		{
			var appointment = await _context.SpaServiceCategories
				.AsNoTracking() // Không theo dõi thay đổi của thực thể
				.FirstOrDefaultAsync(s => s.Id == id);

			if (appointment == null)
				return new APIErrorResult<SpaServiceCategoryModel>(Messages.NotFoundGet);

			var appointmentModel = DataHelper.Mapping<SpaServiceCategory, SpaServiceCategoryModel>(appointment);

			return new APISuccessResult<SpaServiceCategoryModel>(Messages.GetResultSuccess, appointmentModel);
		}

		public async Task<APIBaseResult<PagingResult<SpaServiceCategoryModel>>> GetPaging(Filter filter)
		{
			IQueryable<SpaServiceCategory> query = _context.SpaServiceCategories.AsNoTracking(); // Không theo dõi thay đổi của thực thể

			query = query.ApplyCommonFilters(filter);

			var totalCount = await query.CountAsync();

			query = query.ApplyPaging(filter);

			var listSpaServiceCategory = await query
				.OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
				.ToListAsync();

			var listSpaServiceCategoryModel = DataHelper.MappingList<SpaServiceCategory, SpaServiceCategoryModel>(listSpaServiceCategory);
			var pageResult = new PagingResult<SpaServiceCategoryModel>()
			{
				TotalRecord = totalCount,
				PageRecord = listSpaServiceCategoryModel.Count(),
				PageIndex = filter.PageIndex,
				PageSize = filter.PageSize,
				Items = listSpaServiceCategoryModel
			};

			return new APISuccessResult<PagingResult<SpaServiceCategoryModel>>(Messages.GetListResultSuccess, pageResult);
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

			var listSpaServiceCategoryModel = new List<SpaServiceCategoryModel>();

			for (int i = 1; i <= sheet.LastRowNum; i++)
			{
				// Row data
				var row = sheet.GetRow(i);
				if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
					continue; // Bỏ qua hàng trống

				SpaServiceCategoryModel appointmentModel = DataHelper.CopyImport<SpaServiceCategoryModel>(headerRow, row);
				listSpaServiceCategoryModel.Add(appointmentModel);
			}

			var listSpaServiceCategory = new List<SpaServiceCategory>();
			DataHelper.MapListAudit<SpaServiceCategoryModel, SpaServiceCategory>(listSpaServiceCategoryModel, listSpaServiceCategory, _currentUser.UserName);

			await _context.SpaServiceCategories.AddRangeAsync(listSpaServiceCategory);

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.ImportSuccess, true);

			return new APIErrorResult<bool>(Messages.ImportFailed);
		}

		public async Task<APIBaseResult<byte[]>> Export(Filter filter)
		{
			var data = await GetPaging(filter);
			var items = data?.Result?.Items ?? new List<SpaServiceCategoryModel>();

			using var workbook = new XLWorkbook();
			var worksheet = workbook.Worksheets.Add(TableName.SpaServiceCategory);

			var listSpaServiceCategory = DataHelper.MappingList<SpaServiceCategoryModel, SpaServiceCategory>(items);
			DataHelper.CopyExport(worksheet, listSpaServiceCategory);

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
