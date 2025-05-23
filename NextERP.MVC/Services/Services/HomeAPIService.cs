using Newtonsoft.Json;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.MVC.Services.Interfaces;
using NextERP.Util;
using System.Text;

namespace NextERP.MVC.Services.Services
{
    public class HomeAPIService : BaseAPIService, IHomeAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public HomeAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }
    }
}
