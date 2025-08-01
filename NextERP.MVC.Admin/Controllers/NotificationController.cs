﻿using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class NotificationController : BaseController
    {
        #region Infrastructure

        private readonly INotificationAPIService _notificationAPIService;

        public NotificationController(INotificationAPIService notificationAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _notificationAPIService = notificationAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult NotificationIndex()
        {
            return View(new NotificationModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _notificationAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(Filter filter)
        {
            var result = await _notificationAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(Localization(result.Message));

            return PartialView(ScreenName.Notification.NotificationList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(NotificationModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _notificationAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _notificationAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _notificationAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _notificationAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(Filter filter)
        {
            var result = await _notificationAPIService.Export(filter);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.Notification, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
