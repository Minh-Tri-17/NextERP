using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using System.Text.RegularExpressions;

namespace NextERP.MVC.Admin.Controllers
{
    public class SupplierOrderController : BaseController
    {
        #region Infrastructure

        private readonly ISupplierOrderAPIService _supplierOrderAPIService;
        private readonly ISharedCultureLocalizer _localizer;

        public SupplierOrderController(ISupplierOrderAPIService supplierOrderAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _supplierOrderAPIService = supplierOrderAPIService;
            _localizer = localizer;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult SupplierOrderIndex()
        {
            return View(new SupplierOrderModel());
        }

        [HttpPost]
        public async Task<ActionResult> GetList(SupplierOrderModel request)
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
                        FilterName = SupplierOrderModel.AttributeNames.SupplierOrderCode,
                        FilterValue = DataHelper.GetString(request.SupplierOrderCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = SupplierOrderModel.AttributeNames.OrderStatus,
                        FilterValue = DataHelper.GetString(request.OrderStatus),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Contains.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = SupplierOrderModel.AttributeNames.PaymentStatus,
                        FilterValue = DataHelper.GetString(request.PaymentStatus),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Contains.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = SupplierOrderModel.AttributeNames.OrderDate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.OrderDate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = SupplierOrderModel.AttributeNames.ExpectedDeliveryDate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.ExpectedDeliveryDate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = SupplierOrderModel.AttributeNames.ActualDeliveryDate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.ActualDeliveryDate)),
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

            var result = await _supplierOrderAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            foreach (var supplierOrder in result!.Result!.Items!)
            {
                var imageBytes = await _supplierOrderAPIService.GetImageBytes(DataHelper.GetGuid(supplierOrder.Id), DataHelper.GetString(supplierOrder.ImagePath));

                if (imageBytes.Length > 0)
                {
                    using (var image = Image.Load<Rgba32>(imageBytes, out IImageFormat format))
                    {
                        using (var ms = new MemoryStream())
                        {
                            image.Save(ms, format); // Lưu lại đúng định dạng gốc
                            string base64Image = $"data:{format.DefaultMimeType};base64,{Convert.ToBase64String(ms.ToArray())}";
                            supplierOrder.Base64Image = base64Image;
                        }
                    }
                }
            }

            return PartialView(ScreenName.SupplierOrder.SupplierOrderList, result);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _supplierOrderAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _supplierOrderAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _supplierOrderAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(SupplierOrderModel request)
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
                        FilterName = SupplierOrderModel.AttributeNames.SupplierOrderCode,
                        FilterValue = DataHelper.GetString(request.SupplierOrderCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = SupplierOrderModel.AttributeNames.OrderStatus,
                        FilterValue = DataHelper.GetString(request.OrderStatus),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Contains.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = SupplierOrderModel.AttributeNames.PaymentStatus,
                        FilterValue = DataHelper.GetString(request.PaymentStatus),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Contains.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = SupplierOrderModel.AttributeNames.OrderDate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.OrderDate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = SupplierOrderModel.AttributeNames.ExpectedDeliveryDate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.ExpectedDeliveryDate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = SupplierOrderModel.AttributeNames.ActualDeliveryDate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.ActualDeliveryDate)),
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

            var result = await _supplierOrderAPIService.Export(filter);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.SupplierOrder, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        [HttpPost]
        public async Task<ActionResult> Signature(SupplierOrderModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            if (!string.IsNullOrWhiteSpace(request.Base64Image))
            {
                // Cắt bỏ prefix "data:image/png;base64,"
                var base64Data = Regex.Replace(request.Base64Image, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

                var bytes = Convert.FromBase64String(base64Data);

                // KHÔNG dùng using, để stream còn tồn tại khi gọi OpenReadStream
                var stream = new MemoryStream(bytes);
                IFormFile file = new FormFile(stream, 0, bytes.Length, "ImageFile", "signature.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                request.ImageFile = file;
            }

            var result = await _supplierOrderAPIService.Signature(request);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        #endregion
    }
}
