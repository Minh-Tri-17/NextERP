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
        public async Task<ActionResult> GetList(Filter filter)
        {
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
