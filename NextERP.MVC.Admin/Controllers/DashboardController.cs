using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NextERP.MVC.Admin.Services.Interfaces;

namespace NextERP.MVC.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        #region Infrastructure

        private readonly IDashboardAPIService _dashboardAPIService;

        public DashboardController(IDashboardAPIService dashboardAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _dashboardAPIService = dashboardAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult DashboardIndex()
        {
            ViewBag.UserId = GetInfor()[0];
            return View();
        }

        #endregion

        #region Custom Operations

        [HttpGet]
        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return LocalRedirect(returnUrl);
        }

        #endregion
    }
}
