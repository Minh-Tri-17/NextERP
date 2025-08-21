using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Interface
{
    public interface IDashboardService
    {
        public Task<APIBaseResult<DataChartNumeric>> GetChartColumn();
        public Task<APIBaseResult<DataChartSingle>> GetChartDonut();
        public Task<APIBaseResult<DataChartNumeric>> GetChartRadar();
        public Task<APIBaseResult<DataChartSingle>> GetChartLine();
        public Task<APIBaseResult<DataChartXY>> GetChartSlope();
        public Task<APIBaseResult<DataChartSingle>> GetChartFunnel();
        public Task<APIBaseResult<decimal>> GetStatisticsProfit();
        public Task<APIBaseResult<decimal>> GetStatisticsRevenue();
        public Task<APIBaseResult<decimal>> GetStatisticsSpending();
        public Task<APIBaseResult<int>> GetStatisticsCustomer();
        public Task<APIBaseResult<List<string>>> GetStatisticsService();
    }
}
