using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.Util;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Service
{
	public class BranchService : IBranchService
	{
		#region Infrastructure

		private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
		private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

		public BranchService(NextErpContext context, ICurrentUserService currentUser)
		{
			_context = context;
			_currentUser = currentUser;
		}

		#endregion

		#region Default Operations

		public async Task<APIBaseResult<bool>> CreateOrEdit(BranchModel request)
		{
			#region Check null request and create variable

			var id = DataHelper.GetGuid(request.Id);

			#endregion

			if (id == Guid.Empty)
			{
				var branch = new Branch();
				DataHelper.MapAudit(request, branch, _currentUser.UserName);

				await _context.Branches.AddAsync(branch);

				var result = await _context.SaveChangesAsync();
				if (result > 0)
					return new APISuccessResult<bool>(Messages.CreateSuccess, true);

				return new APIErrorResult<bool>(Messages.CreateFailed);
			}
			else
			{
				var branch = await _context.Branches.FindAsync(id);
				if (branch == null)
					return new APIErrorResult<bool>(Messages.NotFoundUpdate);

				DataHelper.MapAudit(request, branch, _currentUser.UserName);

				var result = await _context.SaveChangesAsync();
				if (result > 0)
					return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

				return new APIErrorResult<bool>(Messages.UpdateFailed);
			}
		}

		public async Task<APIBaseResult<bool>> Delete(string ids)
		{
			List<Guid> listBranchId = ids.Split(',')
				.Select(id => DataHelper.GetGuid(id.Trim()))
				.Where(guid => guid != Guid.Empty)
				.ToList();

			var listBranch = await _context.Branches
				.Where(s => listBranchId.Contains(s.Id))
				.ToListAsync();

			foreach (var branch in listBranch)
			{
				branch.IsDelete = true; // Đánh dấu là đã xóa
			}

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

			return new APIErrorResult<bool>(Messages.DeleteFailed);
		}

		public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
		{
			List<Guid> listBranchId = ids.Split(',')
				 .Select(id => DataHelper.GetGuid(id.Trim()))
				 .Where(guid => guid != Guid.Empty)
				 .ToList();

			var listBranch = await _context.Branches
				.Where(s => listBranchId.Contains(s.Id))
				.ToListAsync();

			foreach (var branch in listBranch)
			{
				_context.Branches.Remove(branch); // Xóa vĩnh viễn
			}

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

			return new APIErrorResult<bool>(Messages.DeleteFailed);
		}

		public async Task<APIBaseResult<BranchModel>> GetOne(Guid id)
		{
			var branch = await _context.Branches
				.AsNoTracking() // Không theo dõi thay đổi của thực thể
				.FirstOrDefaultAsync(s => s.Id == id);

			if (branch == null)
				return new APIErrorResult<BranchModel>(Messages.NotFoundGet);

			var branchModel = DataHelper.Mapping<Branch, BranchModel>(branch);

			return new APISuccessResult<BranchModel>(Messages.GetResultSuccess, branchModel);
		}

		public async Task<APIBaseResult<PagingResult<BranchModel>>> GetPaging(FilterModel filter)
		{
			IQueryable<Branch> query = _context.Branches.AsNoTracking(); // Không theo dõi thay đổi của thực thể

			query = query.ApplyCommonFilters(filter);

			var totalCount = await query.CountAsync();

			query = query.ApplyPaging(filter);

			var listBranch = await query
				.OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
				.ToListAsync();

			var listBranchModel = DataHelper.MappingList<Branch, BranchModel>(listBranch);
			var pageResult = new PagingResult<BranchModel>()
			{
				TotalRecord = totalCount,
				PageRecord = listBranchModel.Count(),
				PageIndex = filter.PageIndex,
				PageSize = filter.PageSize,
				Items = listBranchModel
			};

			return new APISuccessResult<PagingResult<BranchModel>>(Messages.GetListResultSuccess, pageResult);
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

			var listBranchModel = new List<BranchModel>();

			for (int i = 1; i <= sheet.LastRowNum; i++)
			{
				// Row data
				var row = sheet.GetRow(i);
				if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
					continue; // Bỏ qua hàng trống

				BranchModel branchModel = DataHelper.CopyImport<BranchModel>(headerRow, row);
				listBranchModel.Add(branchModel);
			}

			var listBranch = new List<Branch>();
			DataHelper.MapListAudit<BranchModel, Branch>(listBranchModel, listBranch, _currentUser.UserName);

			await _context.Branches.AddRangeAsync(listBranch);

			var result = await _context.SaveChangesAsync();
			if (result > 0)
				return new APISuccessResult<bool>(Messages.ImportSuccess, true);

			return new APIErrorResult<bool>(Messages.ImportFailed);
		}

		public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
		{
			var data = await GetPaging(filter);
			var items = data?.Result?.Items ?? new List<BranchModel>();

			using var workbook = new XLWorkbook();
			var worksheet = workbook.Worksheets.Add(TableName.Branch);

			var listBranch = DataHelper.MappingList<BranchModel, Branch>(items);
			DataHelper.CopyExport(worksheet, listBranch);

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
