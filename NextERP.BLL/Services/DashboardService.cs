using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Service
{
    public class DashboardService : IDashboardService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public DashboardService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        #endregion

        #region Custom Operations

        #region Statistics

        public async Task<APIBaseResult<decimal>> GetStatisticsProfit()
        {
            return new APISuccessResult<decimal>(Messages.GetListResultSuccess, 12628);
        }

        public async Task<APIBaseResult<decimal>> GetStatisticsRevenue()
        {
            return new APISuccessResult<decimal>(Messages.GetListResultSuccess, 14679);
        }

        public async Task<APIBaseResult<decimal>> GetStatisticsSpending()
        {
            return new APISuccessResult<decimal>(Messages.GetListResultSuccess, 56575);
        }

        public async Task<APIBaseResult<int>> GetStatisticsCustomer()
        {
            return new APISuccessResult<int>(Messages.GetListResultSuccess, 246);
        }

        public async Task<APIBaseResult<List<string>>> GetStatisticsService()
        {
            var listService = new List<string>
            {
                "1. massage thái - 1,200 khách/tháng",
                "2. chăm sóc da mặt - 950 khách/tháng",
                "3. gội đầu dưỡng sinh - 1,500 khách/tháng",
                "4. tẩy tế bào chết - 780 khách/tháng",
                "5. massage đá nóng - 890 khách/tháng",
                "6. xông hơi thảo dược - 1,100 khách/tháng",
                "7. trị liệu giảm đau vai gáy - 970 khách/tháng",
                "8. chăm sóc móng tay & chân - 860 khách/tháng",
                "9. nâng cơ trẻ hóa da - 720 khách/tháng",
                "10. massage bấm huyệt - 1,050 khách/tháng",
            };

            return new APISuccessResult<List<string>>(Messages.GetListResultSuccess, listService);
        }

        #endregion

        #region Chart

        public async Task<APIBaseResult<DataChartNumericModel>> GetChartColumn()
        {
            var dataChart = new DataChartNumericModel
            {
                Values = new List<NumericSeriesModel>
                {
                    new NumericSeriesModel {
                        Name  = "Net Profit",
                        Data  = new int [] { 44, 55, 57, 56, 61, 58, 63, 60, 66 }
                    },
                    new NumericSeriesModel {
                        Name = "Revenue",
                        Data = new int [] { 76, 85, 101, 98, 87, 105, 91, 114, 94 }
                    },
                    new NumericSeriesModel {
                        Name = "Free Cash Flow",
                        Data = new int[] { 35, 41, 36, 26, 45, 48, 52, 53, 41 }
                    },
                },
                Labels = new string[] { "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct" }
            };

            return new APISuccessResult<DataChartNumericModel>(Messages.GetListResultSuccess, dataChart);
        }

        public async Task<APIBaseResult<DataChartSingleModel>> GetChartDonut()
        {
            var dataChart = new DataChartSingleModel
            {
                Values = new int[] { 44, 55, 41, 17, 15 },
                Labels = new string[] { "series-1", "series-2", "series-3", "series-4", "series-5" }
            };

            return new APISuccessResult<DataChartSingleModel>(Messages.GetListResultSuccess, dataChart);
        }

        public async Task<APIBaseResult<DataChartSingleModel>> GetChartFunnel()
        {
            var dataChart = new DataChartSingleModel
            {
                Values = new int[] { 200, 330, 548, 740, 880, 990, 1100, 1380 },
                Labels = new string[] { "Sweets", "Processed Foods", "Healthy Fats", "Meat", "Beans & Legumes", "Dairy", "Fruits & Vegetables", "Grains" }
            };

            return new APISuccessResult<DataChartSingleModel>(Messages.GetListResultSuccess, dataChart);
        }

        public async Task<APIBaseResult<DataChartSingleModel>> GetChartLine()
        {
            var dataChart = new DataChartSingleModel
            {
                Values = new int[] { 10, 41, 35, 51, 49, 62, 69, 91, 148 },
                Labels = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep" }
            };

            return new APISuccessResult<DataChartSingleModel>(Messages.GetListResultSuccess, dataChart);
        }

        public async Task<APIBaseResult<DataChartNumericModel>> GetChartRadar()
        {
            var dataChart = new DataChartNumericModel
            {
                Values = new List<NumericSeriesModel>
                {
                    new NumericSeriesModel
                    {
                        Name  = "Series-1",
                        Data  = new int [] { 80, 50, 30, 40, 100, 20 }
                    },
                    new NumericSeriesModel
                    {
                        Name  = "Series-2",
                        Data  = new int [] { 20, 30, 40, 80, 20, 80 }
                    },
                    new NumericSeriesModel
                    {
                        Name  = "Series-3",
                        Data  = new int[] { 44, 76, 78, 13, 43, 10 }
                    },
                },
                Labels = new string[] { "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct" }
            };

            return new APISuccessResult<DataChartNumericModel>(Messages.GetListResultSuccess, dataChart);
        }

        public async Task<APIBaseResult<DataChartXYModel>> GetChartSlope()
        {
            var dataChart = new DataChartXYModel
            {
                Values = new List<XYSeriesModel>
                {
                    new XYSeriesModel{
                        Name  = "Blue",
                        Data = new List<XYPointModel>
                        {
                            new XYPointModel {X = "Category 1", Y = 503},
                            new XYPointModel {X = "Category 2", Y = 580},
                            new XYPointModel {X = "Category 3", Y = 135},
                            new XYPointModel {X = "Category 4", Y = 363},
                        }
                    },
                    new XYSeriesModel{
                        Name  = "Green",
                        Data = new List<XYPointModel>
                        {
                            new XYPointModel {X = "Category 1", Y = 733},
                            new XYPointModel {X = "Category 2", Y = 385},
                            new XYPointModel {X = "Category 3", Y = 715},
                            new XYPointModel {X = "Category 4", Y = 952},
                        }
                    },
                    new XYSeriesModel{
                        Name  = "Orange",
                        Data = new List<XYPointModel>
                        {
                            new XYPointModel {X = "Category 1", Y = 255},
                            new XYPointModel {X = "Category 2", Y = 211},
                            new XYPointModel {X = "Category 3", Y = 441},
                            new XYPointModel {X = "Category 4", Y = 642},
                        }
                    },
                    new XYSeriesModel{
                        Name  = "Red",
                        Data = new List<XYPointModel>
                        {
                            new XYPointModel {X = "Category 1", Y = 428},
                            new XYPointModel {X = "Category 2", Y = 749},
                            new XYPointModel {X = "Category 3", Y = 559},
                            new XYPointModel {X = "Category 4", Y = 748},
                        }
                    },
                },
                Labels = new string[] { }
            };

            return new APISuccessResult<DataChartXYModel>(Messages.GetListResultSuccess, dataChart);
        }

        #endregion

        #endregion
    }
}
