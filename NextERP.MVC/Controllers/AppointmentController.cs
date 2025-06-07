using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IAttendanceAPIService _attendanceAPIService;

        public AppointmentController(IAttendanceAPIService attendanceAPIService, IConfiguration configuration) : base(configuration)
        {
            _attendanceAPIService = attendanceAPIService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new AttendanceModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _attendanceAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(result.Message);

            var attendance = new AttendanceModel();
            DataHelper.MapAudit(result.Result, attendance, string.Empty);

            return Json(attendance);
        }

        [HttpGet]
        public async Task<ActionResult> GetCategories(Filter filter)
        {
            var result = await _attendanceAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(result.Message);

            return PartialView(Constants.List, result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(AttendanceModel request)
        {
            var result = await _attendanceAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
            {
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        result.Message += $" - {error.ErrorMessage}";
                    }
                }

                return Json(result.Message);
            }

            return RedirectToAction(Constants.Index);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _attendanceAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(result.Message);

            return RedirectToAction(Constants.Index);
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _attendanceAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(result.Message);

            return RedirectToAction(Constants.Index);
        }

        [HttpPost]
        public async Task<ActionResult> Export(Filter filter)
        {
            var result = await _attendanceAPIService.Export(filter);
            if (!DataHelper.IsNotNull(result))
                return Json(result.Message);

            var fileName = string.Format(Constants.FileName, ObjectNames.Appointment, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result, Constants.ContentType, fileName);
        }
    }
}
