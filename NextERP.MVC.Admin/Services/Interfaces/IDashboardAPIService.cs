using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface IDashboardAPIService
    {
        public Task<APIBaseResult<DataChartNumericModel>> GetChartColumn();
        public Task<APIBaseResult<DataChartSingleModel>> GetChartDonut();
        public Task<APIBaseResult<DataChartNumericModel>> GetChartRadar();
        public Task<APIBaseResult<DataChartSingleModel>> GetChartLine();
        public Task<APIBaseResult<DataChartXYModel>> GetChartSlope();
        public Task<APIBaseResult<DataChartSingleModel>> GetChartFunnel();
        public Task<APIBaseResult<decimal>> GetStatisticsProfit();
        public Task<APIBaseResult<decimal>> GetStatisticsRevenue();
        public Task<APIBaseResult<decimal>> GetStatisticsSpending();
        public Task<APIBaseResult<int>> GetStatisticsCustomer();
        public Task<APIBaseResult<List<string>>> GetStatisticsService();
    }
}
