using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class InvoiceController : BaseController
    {
        #region Infrastructure

        private readonly IInvoiceAPIService _invoiceAPIService;

        public InvoiceController(IInvoiceAPIService invoiceAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _invoiceAPIService = invoiceAPIService;
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
                return Json(Localization(result.Message));

            return Json(result.Result);
        }

        [HttpGet]
        public async Task<ActionResult> GetList(Filter filter)
        {
            var result = await _invoiceAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(Localization(result.Message));

            return PartialView(ScreenName.Invoice.InvoiceList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(InvoiceModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _invoiceAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _invoiceAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _invoiceAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
