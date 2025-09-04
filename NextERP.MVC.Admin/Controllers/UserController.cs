using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class UserController : BaseController
    {
        #region Infrastructure

        private readonly IUserAPIService _userAPIService;
        private readonly IEmployeeAPIService _employeeAPIService;

        public UserController(IUserAPIService userAPIService, IEmployeeAPIService employeeAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _userAPIService = userAPIService;
            _employeeAPIService = employeeAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public async Task<IActionResult> UserIndexAsync()
        {
            var filterEmployee = new EmployeeModel();
            var listEmployee = await _employeeAPIService.GetPaging(filterEmployee);
            if (DataHelper.ListIsNotNull(listEmployee))
                ViewBag.ListEmployee = listEmployee!.Result!.Items;

            return View(new UserModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _userAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(UserModel request)
        {
            var result = await _userAPIService.GetPaging(request);
            if (!DataHelper.ListIsNotNull(result))
                return Json(Localization(result.Message));

            return PartialView(ScreenName.User.UserList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(UserModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _userAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _userAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _userAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _userAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(UserModel request)
        {
            var result = await _userAPIService.Export(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.User, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
