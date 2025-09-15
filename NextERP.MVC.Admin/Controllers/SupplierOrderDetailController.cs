using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class SupplierOrderDetailController : BaseController
    {
        #region Infrastructure

        private readonly ISupplierOrderDetailAPIService _supplierOrderDetailAPIService;

        public SupplierOrderDetailController(ISupplierOrderDetailAPIService supplierOrderDetailAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _supplierOrderDetailAPIService = supplierOrderDetailAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult SupplierOrderDetailIndex()
        {
            return View(new SupplierOrderDetailModel());
        }

        [HttpPost]
        public async Task<ActionResult> GetList(SupplierOrderDetailModel request)
        {
            FilterModel filter = new FilterModel()
            {
                Filters = new List<FilterItemModel>()
                {
                    new FilterItemModel()
                    {
                        FilterName = AttributeNames.SupplierOrderDetail.SupplierOrderDetailCode,
                        FilterValue = DataHelper.GetString(request.SupplierOrderDetailCode),
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
                IdMain = request.SupplierOrderId,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };

            var result = await _supplierOrderDetailAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(Localization(result.Message));

            return PartialView(ScreenName.SupplierOrderDetail.SupplierOrderDetailList, result);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _supplierOrderDetailAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
