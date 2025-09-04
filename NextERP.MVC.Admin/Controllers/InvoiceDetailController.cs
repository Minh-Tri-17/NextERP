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

        public InvoiceDetailController(IInvoiceDetailAPIService invoiceDetailAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _invoiceDetailAPIService = invoiceDetailAPIService;
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
                return Json(Localization(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(InvoiceDetailModel request)
        {
            var result = await _invoiceDetailAPIService.GetPaging(request);
            if (!DataHelper.ListIsNotNull(result))
                return Json(Localization(result.Message));

            return PartialView(ScreenName.InvoiceDetail.InvoiceDetailList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(InvoiceDetailModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _invoiceDetailAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _invoiceDetailAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
