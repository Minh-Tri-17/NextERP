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
	public class SpaServiceService : ISpaServiceService
	{
		#region Infrastructure

		private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
		private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

		public SpaServiceService(NextErpContext context, ICurrentUserService currentUser)
		{
			_context = context;
			_currentUser = currentUser;
		}

		#endregion

		#region Default Operations

		public async Task<APIBaseResult<bool>> CreateOrEdit(SpaServiceModel request)
		{
			#region Check null request and create variable

			var id = DataHelper.GetGuid(request.Id);

			#endregion

			if (id == Guid.Empty)
			{
				var spaService = new SpaService();
				DataHelper.MapAudit(request, spaService, _currentUser.UserName);

				await _context.SpaServices.AddAsync(spaService);

				var result = await _context.SaveChangesAsync();
				if (result > 0)
					return new APISuccessResult<bool>(Messages.CreateSuccess, true);

				return new APIErrorResult<bool>(Messages.CreateFailed);
			}
			else
			{
				var spaService = await _context.SpaServices.FindAsync(id);
				if (spaService == null)
					return new APIErrorResult<bool>(Messages.NotFoundUpdate);

				DataHelper.MapAudit(request, spaService, _currentUser.UserName);

				var result = await _context.SaveChangesAsync();
				if (result > 0)
					return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

				return new APIErrorResult<bool>(Messages.UpdateFailed);
			}
		}

		public async Task<APIBaseResult<bool>> Delete(string ids)
		{
			List<Guid> listSpaServiceId = ids.Split(',')
				.Select(id => DataHelper.GetGuid(id.Trim()))
				.Where(guid => guid != Guid.Empty)
				.ToList();

			var listSpaService = await _context.SpaServices
				.Where(s => listSpaServiceId.Contains(s.Id))
				.ToListAsync();

			foreach (var spaService in listSpaService)
			{
				spaService.IsDelete = true; // Đánh dấu là đã xóa
			}

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

			return new APIErrorResult<bool>(Messages.DeleteFailed);
		}

		public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
		{
			List<Guid> listSpaServiceId = ids.Split(',')
			   .Select(id => DataHelper.GetGuid(id.Trim()))
			   .Where(guid => guid != Guid.Empty)
			   .ToList();

			var listSpaService = await _context.SpaServices
				.Where(s => listSpaServiceId.Contains(s.Id))
				.ToListAsync();

			foreach (var spaService in listSpaService)
			{
				_context.SpaServices.Remove(spaService); // Xóa vĩnh viễn
			}

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

			return new APIErrorResult<bool>(Messages.DeleteFailed);
		}

		public async Task<APIBaseResult<SpaServiceModel>> GetOne(Guid id)
		{
			var spaService = await _context.SpaServices
				.AsNoTracking() // Không theo dõi thay đổi của thực thể
				.FirstOrDefaultAsync(s => s.Id == id);

			if (spaService == null)
				return new APIErrorResult<SpaServiceModel>(Messages.NotFoundGet);

			var spaServiceModel = DataHelper.Mapping<SpaService, SpaServiceModel>(spaService);

			return new APISuccessResult<SpaServiceModel>(Messages.GetResultSuccess, spaServiceModel);
		}

		public async Task<APIBaseResult<PagingResult<SpaServiceModel>>> GetPaging(FilterModel filter)
		{
			IQueryable<SpaService> query = _context.SpaServices.AsNoTracking(); // Không theo dõi thay đổi của thực thể

			query = query.ApplyCommonFilters(filter);

			var totalCount = await query.CountAsync();

			query = query.ApplyPaging(filter);

			var listSpaService = await query
				.OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
				.ToListAsync();

			var listSpaServiceModel = DataHelper.MappingList<SpaService, SpaServiceModel>(listSpaService);
			var pageResult = new PagingResult<SpaServiceModel>()
			{
				TotalRecord = totalCount,
				PageRecord = listSpaServiceModel.Count(),
				PageIndex = filter.PageIndex,
				PageSize = filter.PageSize,
				Items = listSpaServiceModel
			};

			return new APISuccessResult<PagingResult<SpaServiceModel>>(Messages.GetListResultSuccess, pageResult);
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

			var listSpaServiceModel = new List<SpaServiceModel>();

			for (int i = 1; i <= sheet.LastRowNum; i++)
			{
				// Row data
				var row = sheet.GetRow(i);
				if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
					continue; // Bỏ qua hàng trống

				SpaServiceModel spaServiceModel = DataHelper.CopyImport<SpaServiceModel>(headerRow, row);
				listSpaServiceModel.Add(spaServiceModel);
			}

			var listSpaService = new List<SpaService>();
			DataHelper.MapListAudit<SpaServiceModel, SpaService>(listSpaServiceModel, listSpaService, _currentUser.UserName);

			await _context.SpaServices.AddRangeAsync(listSpaService);

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.ImportSuccess, true);

			return new APIErrorResult<bool>(Messages.ImportFailed);
		}

		public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
		{
			var data = await GetPaging(filter);
			var items = data?.Result?.Items ?? new List<SpaServiceModel>();

			using var workbook = new XLWorkbook();
			var worksheet = workbook.Worksheets.Add(TableName.SpaService);

			var listSpaService = DataHelper.MappingList<SpaServiceModel, SpaService>(items);
			DataHelper.CopyExport(worksheet, listSpaService);

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
