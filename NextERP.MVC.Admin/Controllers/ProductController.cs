using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace NextERP.MVC.Admin.Controllers
{
    public class ProductController : BaseController
    {
        #region Infrastructure

        private readonly IProductAPIService _productAPIService;

        public ProductController(IProductAPIService productAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _productAPIService = productAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult ProductIndex()
        {
            return View(new ProductModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _productAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(ProductModel request)
        {
            FilterModel filter = new FilterModel()
            {
                Filters = new List<FilterItemModel>()
                {
                    new FilterItemModel()
                    {
                        FilterName = Constants.IsDelete,
                        FilterType = Util.Enums.FilterType.Boolean.ToString(),
                        FilterOperator = DataHelper.GetBool(request.IsDelete)
                            ? Util.Enums.FilterOperator.Equal.ToString()
                            : Util.Enums.FilterOperator.NotEqual.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = AttributeNames.Product.ProductCode,
                        FilterValue = DataHelper.GetString(request.ProductCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = AttributeNames.Product.ExpirationDate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.ExpirationDate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = Constants.DateCreate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.DateCreate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = Constants.DateUpdate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.DateUpdate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                },
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };

            var result = await _productAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(Localization(result.Message));

            foreach (var product in result!.Result!.Items!)
            {
                List<string> base64Images = new List<string>();

                foreach (var productImage in product.ProductImages)
                {
                    var imageBytes = await _productAPIService.GetImageBytes(DataHelper.GetGuid(productImage.ProductId), DataHelper.GetString(productImage.ImagePath));

                    if (imageBytes.Length > 0)
                    {
                        using (var image = Image.Load<Rgba32>(imageBytes, out IImageFormat format))
                        {
                            using (var ms = new MemoryStream())
                            {
                                image.Save(ms, format); // Lưu lại đúng định dạng gốc
                                string base64Image = $"data:{format.DefaultMimeType};base64,{Convert.ToBase64String(ms.ToArray())}";
                                base64Images.Add(base64Image);
                            }
                        }
                    }
                }

                product.Base64Images = base64Images;
            }

            return PartialView(ScreenName.Product.ProductList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(ProductModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _productAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _productAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _productAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _productAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(ProductModel request)
        {
            FilterModel filter = new FilterModel()
            {
                Filters = new List<FilterItemModel>()
                {
                    new FilterItemModel()
                    {
                        FilterName = Constants.IsDelete,
                        FilterType = Util.Enums.FilterType.Boolean.ToString(),
                        FilterOperator = DataHelper.GetBool(request.IsDelete)
                            ? Util.Enums.FilterOperator.Equal.ToString()
                            : Util.Enums.FilterOperator.NotEqual.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = Constants.Ids,
                        FilterValue = DataHelper.GetString(request.Ids),
                        FilterType = Util.Enums.FilterType.Guid.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Contains.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = AttributeNames.Product.ProductCode,
                        FilterValue = DataHelper.GetString(request.ProductCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = AttributeNames.Product.ExpirationDate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.ExpirationDate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = Constants.DateCreate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.DateCreate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = Constants.DateUpdate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.DateUpdate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                },
                AllowPaging = request.AllowPaging,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };

            var result = await _productAPIService.Export(filter);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.Product, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
