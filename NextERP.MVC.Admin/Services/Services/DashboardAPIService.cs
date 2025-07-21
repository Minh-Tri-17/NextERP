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

        #endregion
    }
}
