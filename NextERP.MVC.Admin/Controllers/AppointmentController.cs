using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class AppointmentController : BaseController
    {
        #region Infrastructure

        private readonly IAppointmentAPIService _appointmentAPIService;

        public AppointmentController(IAppointmentAPIService appointmentAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _appointmentAPIService = appointmentAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult AppointmentIndex()
        {
            return View(new AppointmentModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _appointmentAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(AppointmentModel request)
        {
            var result = await _appointmentAPIService.GetPaging(request);
            if (!DataHelper.ListIsNotNull(result))
                return Json(Localization(result.Message));

            var listCalendar = new List<Calendar>();

            foreach (var item in result.Result!.Items!)
            {
                var calendar = new Calendar();

                calendar.Title = DataHelper.GetString(item.Customer?.FullName);
                calendar.Status = DataHelper.GetString(item.AppointmentStatus);
                calendar.Description = DataHelper.GetString(item.Note);
                calendar.Start = DataHelper.GetDateTime(item.AppointmentDate);
                calendar.End = DataHelper.GetDateTime(item.AppointmentDate);
                calendar.AllDay = DataHelper.GetDateTime(item.AppointmentDate).TimeOfDay == TimeSpan.Zero ? true : false;

                listCalendar.Add(calendar);
            }

            return Json(listCalendar);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(AppointmentModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _appointmentAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _appointmentAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _appointmentAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _appointmentAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(AppointmentModel request)
        {
            var result = await _appointmentAPIService.Export(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.Appointment, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
