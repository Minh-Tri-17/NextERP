using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class ScheduleController : BaseController
    {
        #region Infrastructure

        private readonly IScheduleAPIService _scheduleAPIService;

        public ScheduleController(IScheduleAPIService scheduleAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _scheduleAPIService = scheduleAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult ScheduleIndex()
        {
            return View(new ScheduleModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _scheduleAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(Filter filter)
        {
            filter.AllowPaging = false;
            var result = await _scheduleAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(Localization(result.Message));

            var listCalendar = new List<Calendar>();

            foreach (var item in result.Result!.Items!)
            {
                var calendar = new Calendar();

                calendar.Title = DataHelper.GetString(item.Employee?.FullName);
                calendar.Description = DataHelper.GetString(item.Note);
                calendar.Start = DataHelper.GetDateTime(item.WorkDate);
                calendar.End = DataHelper.GetDateTime(item.WorkDate);
                calendar.AllDay = DataHelper.GetDateTime(item.WorkDate).TimeOfDay == TimeSpan.Zero ? true : false;

                listCalendar.Add(calendar);
            }

            return Json(listCalendar);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(ScheduleModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _scheduleAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _scheduleAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _scheduleAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _scheduleAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(Filter filter)
        {
            var result = await _scheduleAPIService.Export(filter);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.Schedule, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
