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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Service
{
    public class ProductService : IProductService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại
        private readonly IStorageService _storageService; // Dùng để lưu trữ file, có thể là trên đám mây hoặc hệ thống tập tin cục bộ

        public ProductService(NextErpContext context, ICurrentUserService currentUser, IStorageService storageService)
        {
            _context = context;
            _currentUser = currentUser;
            _storageService = storageService;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(ProductModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var product = new Product();
                DataHelper.MapAudit(request, product, _currentUser.UserName);

                if (request.ImageFiles != null)
                {
                    var listProductImage = new List<ProductImage>();

                    bool isFirstImage = true;

                    foreach (var file in request.ImageFiles)
                    {
                        var productImage = new ProductImage();
                        var productImageModel = new ProductImageModel()
                        {
                            ProductId = product.Id,
                            IsPrimary = isFirstImage,
                            ImagePath = await SaveFile(file),
                        };

                        DataHelper.MapAudit(productImageModel, productImage, _currentUser.UserName);

                        listProductImage.Add(productImage);

                        isFirstImage = false;
                    }

                    product.ProductImages = listProductImage;
                }

                await _context.Products.AddAsync(product);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, product, _currentUser.UserName);

                if (request.ImageFiles != null && request.ImageFiles.Any())
                {
                    var listImageFileOld = await _context.ProductImages
                        .Where(s => s.ProductId == product.Id)
                        .Select(s => new ProductImageModel
                        {
                            Id = s.Id,
                            ImagePath = s.ImagePath,
                        })
                        .ToListAsync();

                    // Xóa các ảnh củ trong database và trong folder theo $productId
                    if (listImageFileOld.Count > 0)
                    {
                        foreach (var imgaeFileOld in listImageFileOld)
                        {
                            if (imgaeFileOld != null && imgaeFileOld.ImagePath != null)
                            {
                                await _storageService.DeleteFileAsync(imgaeFileOld.ImagePath);
                                _context.ProductImages.Remove(imgaeFileOld);
                            }
                        }
                    }

                    // Tạo ảnh mới vào trong database và trong folder
                    var listProductImage = new List<ProductImage>();

                    bool isFirstImage = true;

                    foreach (var file in request.ImageFiles)
                    {
                        var productImage = new ProductImage();
                        var productImageModel = new ProductImageModel()
                        {
                            ProductId = product.Id,
                            IsPrimary = isFirstImage,
                            ImagePath = await SaveFile(file),
                        };

                        DataHelper.MapAudit(productImageModel, productImage, _currentUser.UserName);

                        listProductImage.Add(productImage);

                        isFirstImage = false;
                    }

                    product.ProductImages = listProductImage;
                }

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listProductId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listProduct = await _context.Products
                .Where(s => listProductId.Contains(s.Id))
                .ToListAsync();

            var listProductID = listProduct.Select(s => s.Id).ToList();
            var listProductImage = await _context.ProductImages
                    .Where(s => s.ProductId.HasValue && listProductID.Contains(s.ProductId.Value))
                    .Select(s => new ProductImageModel
                    {
                        Id = s.Id,
                        ProductId = s.ProductId,
                        ImagePath = s.ImagePath
                    })
                    .ToListAsync();

            foreach (var product in listProduct)
            {
                product.IsDelete = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listProductId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listProduct = await _context.Products
                .Where(s => listProductId.Contains(s.Id))
                .ToListAsync();

            var listProductID = listProduct.Select(s => s.Id).ToList();
            var listProductImage = await _context.ProductImages
                    .Where(s => s.ProductId.HasValue && listProductID.Contains(s.ProductId.Value))
                    .Select(s => new ProductImageModel
                    {
                        Id = s.Id,
                        ProductId = s.ProductId,
                        ImagePath = s.ImagePath
                    })
                    .ToListAsync();

            foreach (var product in listProduct)
            {
                var listProductImageByProduct = listProductImage.Where(s => s.ProductId == product.Id).ToList();

                foreach (var productImageByProduct in listProductImageByProduct)
                {
                    if (productImageByProduct.ImagePath != null)
                        await _storageService.DeleteFileAsync(productImageByProduct.ImagePath);
                }

                _context.ProductImages.RemoveRange(listProductImageByProduct);
                _context.Products.Remove(product);
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<ProductModel>> GetOne(Guid id)
        {
            var product = await _context.Products
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (product == null)
                return new APIErrorResult<ProductModel>(Messages.NotFoundGet);

            var productModel = DataHelper.Mapping<Product, ProductModel>(product);

            return new APISuccessResult<ProductModel>(Messages.GetResultSuccess, productModel);
        }

        public async Task<APIBaseResult<PagingResult<ProductModel>>> GetPaging(ProductModel request)
        {
            IQueryable<Product> query = _context.Products.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            Filter filter = new Filter()
            {
                KeyWord = request.ProductCode,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                IsDelete = DataHelper.GetBool(request.IsDelete)
            };

            query = query.ApplyCommonFilters(filter, s => s.ProductCode!, s => s.IsDelete, s => s.Id);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listProduct = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            foreach (var item in listProduct)
            {
                item.ProductImages = _context.ProductImages.Where(s => s.ProductId.HasValue && s.ProductId == item.Id).ToList();
            }

            var listProductModel = DataHelper.MappingList<Product, ProductModel>(listProduct);
            var pageResult = new PagingResult<ProductModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listProductModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listProductModel
            };

            return new APISuccessResult<PagingResult<ProductModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listProductModel = new List<ProductModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);
                if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
                    continue; // Bỏ qua hàng trống

                ProductModel productModel = DataHelper.CopyImport<ProductModel>(headerRow, row);
                listProductModel.Add(productModel);
            }

            var listProduct = new List<Product>();
            DataHelper.MapListAudit<ProductModel, Product>(listProductModel, listProduct, _currentUser.UserName);

            await _context.Products.AddRangeAsync(listProduct);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(ProductModel request)
        {
            var data = await GetPaging(request);
            var items = data?.Result?.Items ?? new List<ProductModel>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.Product);

            var listProduct = DataHelper.MappingList<ProductModel, Product>(items);
            DataHelper.CopyExport(worksheet, listProduct);

            var stream = new MemoryStream();
            workbook.SaveAs(stream); // Ghi nội dung của workbook(Excel) vào stream
            var bytes = stream.ToArray(); // Chuyển toàn bộ nội dung stream thành mảng byte
            if (bytes.Length > 0)
                return new APISuccessResult<byte[]>(Messages.ExportSuccess, bytes);

            return new APIErrorResult<byte[]>(Messages.ExportFailed);
        }

        #endregion

        #region Custom Operations

        public Task<byte[]> GetImageBytes(Guid productId, string imagePath)
        {
            var fullImagePath = _storageService.GetFileUrl(imagePath);
            byte[] imageData = System.IO.File.ReadAllBytes(fullImagePath);

            return Task.FromResult(imageData);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            string originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{DateTime.Now.ToString(Constants.DateTimeString)}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);

            return fileName;
        }

        #endregion
    }
}
