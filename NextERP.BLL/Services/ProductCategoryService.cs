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
    public class ProductCategoryService : IProductCategoryService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public ProductCategoryService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(ProductCategoryModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var productCategory = new ProductCategory();
                DataHelper.MapAudit(request, productCategory, _currentUser.UserName);

                await _context.ProductCategories.AddAsync(productCategory);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var productCategory = await _context.ProductCategories.FindAsync(id);
                if (productCategory == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, productCategory, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listProductCategoryId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listProductCategory = await _context.ProductCategories
                .Where(s => listProductCategoryId.Contains(s.Id))
                .ToListAsync();

            foreach (var productCategory in listProductCategory)
            {
                productCategory.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listProductCategoryId = ids.Split(',')
                 .Select(id => DataHelper.GetGuid(id.Trim()))
                 .Where(guid => guid != Guid.Empty)
                 .ToList();

            var listProductCategory = await _context.ProductCategories
                .Where(s => listProductCategoryId.Contains(s.Id))
                .ToListAsync();

            foreach (var productCategory in listProductCategory)
            {
                _context.ProductCategories.Remove(productCategory); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<ProductCategoryModel>> GetOne(Guid id)
        {
            var productCategory = await _context.ProductCategories
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (productCategory == null)
                return new APIErrorResult<ProductCategoryModel>(Messages.NotFoundGet);

            var productCategoryModel = DataHelper.Mapping<ProductCategory, ProductCategoryModel>(productCategory);

            return new APISuccessResult<ProductCategoryModel>(Messages.GetResultSuccess, productCategoryModel);
        }

        public async Task<APIBaseResult<PagingResult<ProductCategoryModel>>> GetPaging(ProductCategoryModel request)
        {
            IQueryable<ProductCategory> query = _context.ProductCategories.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            Filter filter = new Filter()
            {
                KeyWord = request.ProductCategoryCode,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                IsDelete = DataHelper.GetBool(request.IsDelete)
            };

            query = query.ApplyCommonFilters(filter, s => s.ProductCategoryCode!, s => s.IsDelete, s => s.Id);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listProductCategory = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listProductCategoryModel = DataHelper.MappingList<ProductCategory, ProductCategoryModel>(listProductCategory);
            var pageResult = new PagingResult<ProductCategoryModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listProductCategoryModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listProductCategoryModel
            };

            return new APISuccessResult<PagingResult<ProductCategoryModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listProductCategoryModel = new List<ProductCategoryModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);
                if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
                    continue; // Bỏ qua hàng trống

                ProductCategoryModel productCategoryModel = DataHelper.CopyImport<ProductCategoryModel>(headerRow, row);
                listProductCategoryModel.Add(productCategoryModel);
            }

            var listProductCategory = new List<ProductCategory>();
            DataHelper.MapListAudit<ProductCategoryModel, ProductCategory>(listProductCategoryModel, listProductCategory, _currentUser.UserName);

            await _context.ProductCategories.AddRangeAsync(listProductCategory);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(ProductCategoryModel request)
        {
            var data = await GetPaging(request);
            var items = data?.Result?.Items ?? new List<ProductCategoryModel>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.ProductCategory);

            var listProductCategory = DataHelper.MappingList<ProductCategoryModel, ProductCategory>(items);
            DataHelper.CopyExport(worksheet, listProductCategory);

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
