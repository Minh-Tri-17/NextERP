using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Đặt ở đây để toàn bộ API đều cần xác thực
    public class DashboardController : ControllerBase
    {
        #region Infrastructure

        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        #endregion

        #region Default Operations

        #endregion

        #region Custom Operations

        [HttpGet(nameof(GetChartColumn))]
        public async Task<ActionResult<IEnumerable<decimal>>> GetChartColumn()
        {
            var result = await _dashboardService.GetChartColumn();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetChartDonut))]
        public async Task<ActionResult<IEnumerable<decimal>>> GetChartDonut()
        {
            var result = await _dashboardService.GetChartDonut();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetChartRadar))]
        public async Task<ActionResult<IEnumerable<decimal>>> GetChartRadar()
        {
            var result = await _dashboardService.GetChartRadar();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetChartLine))]
        public async Task<ActionResult<IEnumerable<decimal>>> GetChartLine()
        {
            var result = await _dashboardService.GetChartLine();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetChartSlope))]
        public async Task<ActionResult<IEnumerable<decimal>>> GetChartSlope()
        {
            var result = await _dashboardService.GetChartSlope();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetChartFunnel))]
        public async Task<ActionResult<IEnumerable<decimal>>> GetChartFunnel()
        {
            var result = await _dashboardService.GetChartFunnel();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetStatisticsProfit))]
        public async Task<ActionResult<IEnumerable<decimal>>> GetStatisticsProfit()
        {
            var result = await _dashboardService.GetStatisticsProfit();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetStatisticsRevenue))]
        public async Task<ActionResult<IEnumerable<decimal>>> GetStatisticsRevenue()
        {
            var result = await _dashboardService.GetStatisticsRevenue();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetStatisticsSpending))]
        public async Task<ActionResult<IEnumerable<decimal>>> GetStatisticsSpending()
        {
            var result = await _dashboardService.GetStatisticsSpending();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetStatisticsCustomer))]
        public async Task<ActionResult<IEnumerable<decimal>>> GetStatisticsCustomer()
        {
            var result = await _dashboardService.GetStatisticsCustomer();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet(nameof(GetStatisticsService))]
        public async Task<ActionResult<IEnumerable<decimal>>> GetStatisticsService()
        {
            var result = await _dashboardService.GetStatisticsService();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion
    }
}
