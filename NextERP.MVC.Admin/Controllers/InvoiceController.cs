using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace NextERP.MVC.Admin.Controllers
{
    public class InvoiceController : BaseController
    {
        #region Infrastructure

        private readonly IInvoiceAPIService _invoiceAPIService;
        private readonly IProductAPIService _productAPIService;
        private readonly IInvoiceDetailAPIService _invoiceDetailAPIService;
        private readonly ISharedCultureLocalizer _localizer;

        public InvoiceController(IInvoiceAPIService invoiceAPIService, IProductAPIService productAPIService,
            IConfiguration configuration, ISharedCultureLocalizer localizer, IInvoiceDetailAPIService invoiceDetailAPIService) : base(configuration, localizer)
        {
            _invoiceAPIService = invoiceAPIService;
            _productAPIService = productAPIService;
            _localizer = localizer;
            _invoiceDetailAPIService = invoiceDetailAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult InvoiceIndex()
        {
            return View(new InvoiceModel());
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _invoiceAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(result.Result);
        }

        [HttpGet]
        public async Task<ActionResult> GetList(InvoiceModel request)
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
                        FilterName = InvoiceModel.AttributeNames.InvoiceCode,
                        FilterValue = DataHelper.GetString(request.InvoiceCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
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

            var result = await _invoiceAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return PartialView(ScreenName.Invoice.InvoiceList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(InvoiceModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _invoiceAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _invoiceAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _invoiceAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        #endregion

        #region Custom Operations

        [HttpPost]
        public async Task<ActionResult> GetProducts(ProductModel request)
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
                        FilterName = ProductModel.AttributeNames.ProductCode,
                        FilterValue = DataHelper.GetString(request.ProductCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = ProductModel.AttributeNames.ExpirationDate,
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
                return Json(_localizer.GetLocalizedString(result.Message));

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

            return PartialView(ScreenName.Invoice.ProductCardList, result);
        }

        public async Task<ActionResult> GetInvoiceDetails(InvoiceDetailModel request)
        {
            FilterModel filter = new FilterModel()
            {
                Filters = new List<FilterItemModel>()
                {
                    new FilterItemModel()
                    {
                        FilterName = InvoiceDetailModel.AttributeNames.InvoiceDetailCode,
                        FilterValue = DataHelper.GetString(request.InvoiceDetailCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
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

            var result = await _invoiceDetailAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return PartialView(ScreenName.InvoiceDetail.InvoiceDetailList, result);
        }

        #endregion
    }
}
