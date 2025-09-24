using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class InvoiceDetailController : BaseController
    {
        #region Infrastructure

        private readonly IInvoiceDetailAPIService _invoiceDetailAPIService;
        private readonly ISharedCultureLocalizer _localizer;

        public InvoiceDetailController(IInvoiceDetailAPIService invoiceDetailAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _invoiceDetailAPIService = invoiceDetailAPIService;
            _localizer = localizer;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult InvoiceDetailIndex()
        {
            return View(new InvoiceDetailModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _invoiceDetailAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(InvoiceDetailModel request)
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

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(InvoiceDetailModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _invoiceDetailAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _invoiceDetailAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
