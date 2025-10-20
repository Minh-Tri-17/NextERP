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
        private readonly ISharedCultureLocalizer _localizer;

        public DashboardController(IDashboardAPIService dashboardAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _dashboardAPIService = dashboardAPIService;
            _localizer = localizer;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public async Task<IActionResult> DashboardIndexAsync()
        {
            var profitTask = _dashboardAPIService.GetStatisticsProfit();
            var revenueTask = _dashboardAPIService.GetStatisticsRevenue();
            var spendingTask = _dashboardAPIService.GetStatisticsSpending();
            var customerTask = _dashboardAPIService.GetStatisticsCustomer();
            var serviceTask = _dashboardAPIService.GetStatisticsService();

            // Await all
            await Task.WhenAll(profitTask, revenueTask, spendingTask, customerTask, serviceTask);

            var dashboardModel = new DashboardModel();

            var profitResp = await profitTask;
            dashboardModel.StatisticsProfit = profitResp?.Result ?? 0m;

            var revenueResp = await revenueTask;
            dashboardModel.StatisticsRevenue = revenueResp?.Result ?? 0m;

            var spendingResp = await spendingTask;
            dashboardModel.StatisticsSpending = spendingResp?.Result ?? 0m;

            var customerResp = await customerTask;
            dashboardModel.StatisticsCustomer = customerResp?.Result ?? 0;

            var serviceResp = await serviceTask;
            dashboardModel.StatisticsService = serviceResp?.Result ?? new List<string>();

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
