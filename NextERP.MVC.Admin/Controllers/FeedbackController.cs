﻿using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class FeedbackController : BaseController
    {
        #region Infrastructure

        private readonly IFeedbackAPIService _feedbackAPIService;

        public FeedbackController(IFeedbackAPIService feedbackAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _feedbackAPIService = feedbackAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult FeedbackIndex()
        {
            return View(new FeedbackModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _feedbackAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(Filter filter)
        {
            var result = await _feedbackAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(Localization(result.Message));

            return PartialView(ScreenName.Feedback.FeedbackList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(FeedbackModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _feedbackAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _feedbackAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _feedbackAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _feedbackAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(Filter filter)
        {
            var result = await _feedbackAPIService.Export(filter);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.Feedback, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
