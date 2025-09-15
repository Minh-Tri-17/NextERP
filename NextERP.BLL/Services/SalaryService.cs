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
	public class SalaryService : ISalaryService
	{
		#region Infrastructure

		private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
		private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

		public SalaryService(NextErpContext context, ICurrentUserService currentUser)
		{
			_context = context;
			_currentUser = currentUser;
		}

		#endregion

		#region Default Operations

		public async Task<APIBaseResult<bool>> CreateOrEdit(SalaryModel request)
		{
			#region Check null request and create variable

			var id = DataHelper.GetGuid(request.Id);

			#endregion

			if (id == Guid.Empty)
			{
				var salary = new Salary();
				DataHelper.MapAudit(request, salary, _currentUser.UserName);

				await _context.Salaries.AddAsync(salary);

				var result = await _context.SaveChangesAsync();
				if (result > 0)
					return new APISuccessResult<bool>(Messages.CreateSuccess, true);

				return new APIErrorResult<bool>(Messages.CreateFailed);
			}
			else
			{
				var salary = await _context.Salaries.FindAsync(id);
				if (salary == null)
					return new APIErrorResult<bool>(Messages.NotFoundUpdate);

				DataHelper.MapAudit(request, salary, _currentUser.UserName);

				var result = await _context.SaveChangesAsync();
				if (result > 0)
					return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

				return new APIErrorResult<bool>(Messages.UpdateFailed);
			}
		}

		public async Task<APIBaseResult<bool>> Delete(string ids)
		{
			List<Guid> listSalaryId = ids.Split(',')
				.Select(id => DataHelper.GetGuid(id.Trim()))
				.Where(guid => guid != Guid.Empty)
				.ToList();

			var listSalary = await _context.Salaries
				.Where(s => listSalaryId.Contains(s.Id))
				.ToListAsync();

			foreach (var salary in listSalary)
			{
				salary.IsDelete = true; // Đánh dấu là đã xóa
			}

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

			return new APIErrorResult<bool>(Messages.DeleteFailed);
		}

		public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
		{
			List<Guid> listSalaryId = ids.Split(',')
				 .Select(id => DataHelper.GetGuid(id.Trim()))
				 .Where(guid => guid != Guid.Empty)
				 .ToList();

			var listSalary = await _context.Salaries
				.Where(s => listSalaryId.Contains(s.Id))
				.ToListAsync();

			foreach (var salary in listSalary)
			{
				_context.Salaries.Remove(salary); // Xóa vĩnh viễn
			}

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

			return new APIErrorResult<bool>(Messages.DeleteFailed);
		}

		public async Task<APIBaseResult<SalaryModel>> GetOne(Guid id)
		{
			var salary = await _context.Salaries
				.AsNoTracking() // Không theo dõi thay đổi của thực thể
				.FirstOrDefaultAsync(s => s.Id == id);

			if (salary == null)
				return new APIErrorResult<SalaryModel>(Messages.NotFoundGet);

			var salaryModel = DataHelper.Mapping<Salary, SalaryModel>(salary);

			return new APISuccessResult<SalaryModel>(Messages.GetResultSuccess, salaryModel);
		}

		public async Task<APIBaseResult<PagingResult<SalaryModel>>> GetPaging(FilterModel filter)
		{
			IQueryable<Salary> query = _context.Salaries.AsNoTracking(); // Không theo dõi thay đổi của thực thể

			query = query.ApplyCommonFilters(filter);

			var totalCount = await query.CountAsync();

			query = query.ApplyPaging(filter);

			var listSalary = await query
				.OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
				.ToListAsync();

			var listSalaryModel = DataHelper.MappingList<Salary, SalaryModel>(listSalary);
			var pageResult = new PagingResult<SalaryModel>()
			{
				TotalRecord = totalCount,
				PageRecord = listSalaryModel.Count(),
				PageIndex = filter.PageIndex,
				PageSize = filter.PageSize,
				Items = listSalaryModel
			};

			return new APISuccessResult<PagingResult<SalaryModel>>(Messages.GetListResultSuccess, pageResult);
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

			var listSalaryModel = new List<SalaryModel>();

			for (int i = 1; i <= sheet.LastRowNum; i++)
			{
				// Row data
				var row = sheet.GetRow(i);
				if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
					continue; // Bỏ qua hàng trống

				SalaryModel salaryModel = DataHelper.CopyImport<SalaryModel>(headerRow, row);
				listSalaryModel.Add(salaryModel);
			}

			var listSalary = new List<Salary>();
			DataHelper.MapListAudit<SalaryModel, Salary>(listSalaryModel, listSalary, _currentUser.UserName);

			await _context.Salaries.AddRangeAsync(listSalary);

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.ImportSuccess, true);

			return new APIErrorResult<bool>(Messages.ImportFailed);
		}

		public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
		{
			var data = await GetPaging(filter);
			var items = data?.Result?.Items ?? new List<SalaryModel>();

			using var workbook = new XLWorkbook();
			var worksheet = workbook.Worksheets.Add(TableName.Salary);

			var listSalary = DataHelper.MappingList<SalaryModel, Salary>(items);
			DataHelper.CopyExport(worksheet, listSalary);

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
