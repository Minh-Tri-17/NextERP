using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
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

            var dashboardModel = new DashboardModel();
            dashboardModel.StatisticsService = _dashboardAPIService.GetStatisticsService().Result.Result ?? new List<string>();
            dashboardModel.StatisticsProfit = _dashboardAPIService.GetStatisticsProfit().Result.Result;
            dashboardModel.StatisticsRevenue = _dashboardAPIService.GetStatisticsRevenue().Result.Result;
            dashboardModel.StatisticsSpending = _dashboardAPIService.GetStatisticsSpending().Result.Result;
            dashboardModel.StatisticsCustomer = _dashboardAPIService.GetStatisticsCustomer().Result.Result;

            return View(dashboardModel);
        }

        #region Get data chart

        public async Task<ActionResult> GetChartColumn()
        {
            var data = await _dashboardAPIService.GetChartColumn();
            return Json(data);
        }

        public async Task<ActionResult> GetChartDonut()
        {
            var data = await _dashboardAPIService.GetChartDonut();
            return Json(data);
        }

        public async Task<ActionResult> GetChartRadar()
        {
            var data = await _dashboardAPIService.GetChartRadar();
            return Json(data);
        }

        public async Task<ActionResult> GetChartLine()
        {
            var data = await _dashboardAPIService.GetChartLine();
            return Json(data);
        }

        public async Task<ActionResult> GetChartSlope()
        {
            var data = await _dashboardAPIService.GetChartSlope();
            return Json(data);
        }

        public async Task<ActionResult> GetChartFunnel()
        {
            var data = await _dashboardAPIService.GetChartFunnel();
            return Json(data);
        }

        #endregion

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
