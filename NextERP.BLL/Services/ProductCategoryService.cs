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
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public ProductCategoryService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(Guid id, ProductCategoryModel request)
        {
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
                .Where(x => listProductCategoryId.Contains(x.Id))
                .ToListAsync();

            foreach (var productCategory in listProductCategory)
            {
                productCategory.IsDelete = true;
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
                .FirstOrDefaultAsync(x => x.Id == id);

            if (productCategory == null)
                return new APIErrorResult<ProductCategoryModel>(Messages.NotFoundGet);

            var productCategoryModel = DataHelper.Mapping<ProductCategory, ProductCategoryModel>(productCategory);

            return new APISuccessResult<ProductCategoryModel>(Messages.GetResultSuccess, productCategoryModel);
        }

        public async Task<APIBaseResult<PagingResult<ProductCategoryModel>>> GetPaging(Filter filter)
        {
            IQueryable<ProductCategory> query = _context.ProductCategories
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.ProductCategoryCode)
                    && x.ProductCategoryCode.ToLower().Contains(keyword));
            }

            var totalCount = await query.CountAsync();

            if (filter.AllowPaging)
            {
                query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            if (!string.IsNullOrEmpty(filter.Ids))
            {
                List<Guid> listProductCategoryId = filter.Ids.Split(',')
                     .Select(id => DataHelper.GetGuid(id.Trim()))
                     .Where(guid => guid != Guid.Empty)
                     .ToList();

                query = query.Where(x => listProductCategoryId.Contains(x.Id));
            }

            var listProductCategory = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .ToListAsync();

            if (!listProductCategory.Any())
                return new APIErrorResult<PagingResult<ProductCategoryModel>>(Messages.NotFoundGetList);

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

                if (row != null)
                {
                    ProductCategoryModel productCategoryModel = DataHelper.CopyImport<ProductCategoryModel>(headerRow, row);
                    listProductCategoryModel.Add(productCategoryModel);
                }
            }

            var listProductCategory = new List<ProductCategory>();
            DataHelper.MapListAudit<ProductCategoryModel, ProductCategory>(listProductCategoryModel, listProductCategory, _currentUser.UserName);

            await _context.ProductCategories.AddRangeAsync(listProductCategory);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            var data = await GetPaging(filter);
            var items = data?.Result?.Items;

            if (items == null || items.Count == 0)
                return new APIErrorResult<byte[]>(Messages.NotFoundGetList);

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
    }
}
