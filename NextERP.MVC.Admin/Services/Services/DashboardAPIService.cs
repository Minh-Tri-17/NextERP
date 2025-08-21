using Newtonsoft.Json;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;
using System.Text;

namespace NextERP.MVC.Admin.Services.Services
{
    public class DashboardAPIService : BaseAPIService, IDashboardAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public DashboardAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        #endregion

        #region Custom Operations

        public async Task<APIBaseResult<DataChartNumeric>> GetChartColumn()
        {
            return await GetAsync<APIBaseResult<DataChartNumeric>>(Constants.UrlGetChartColumn);
        }

        public async Task<APIBaseResult<DataChartSingle>> GetChartDonut()
        {
            return await GetAsync<APIBaseResult<DataChartSingle>>(Constants.UrlGetChartDonut);
        }

        public async Task<APIBaseResult<DataChartNumeric>> GetChartRadar()
        {
            return await GetAsync<APIBaseResult<DataChartNumeric>>(Constants.UrlGetChartRadar);
        }

        public async Task<APIBaseResult<DataChartSingle>> GetChartLine()
        {
            return await GetAsync<APIBaseResult<DataChartSingle>>(Constants.UrlGetChartLine);
        }

        public async Task<APIBaseResult<DataChartXY>> GetChartSlope()
        {
            return await GetAsync<APIBaseResult<DataChartXY>>(Constants.UrlGetChartSlope);
        }

        public async Task<APIBaseResult<DataChartSingle>> GetChartFunnel()
        {
            return await GetAsync<APIBaseResult<DataChartSingle>>(Constants.UrlGetChartFunnel);
        }

        public async Task<APIBaseResult<decimal>> GetStatisticsProfit()
        {
            return await GetAsync<APIBaseResult<decimal>>(Constants.UrlGetStatisticsProfit);
        }

        public async Task<APIBaseResult<decimal>> GetStatisticsRevenue()
        {
            return await GetAsync<APIBaseResult<decimal>>(Constants.UrlGetStatisticsRevenue);
        }

        public async Task<APIBaseResult<decimal>> GetStatisticsSpending()
        {
            return await GetAsync<APIBaseResult<decimal>>(Constants.UrlGetStatisticsSpending);
        }

        public async Task<APIBaseResult<int>> GetStatisticsCustomer()
        {
            return await GetAsync<APIBaseResult<int>>(Constants.UrlGetStatisticsCustomer);
        }

        public async Task<APIBaseResult<List<string>>> GetStatisticsService()
        {
            return await GetAsync<APIBaseResult<List<string>>>(Constants.UrlGetStatisticsService);
        }

        #endregion
    }
}
