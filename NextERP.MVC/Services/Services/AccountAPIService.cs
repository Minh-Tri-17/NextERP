using Newtonsoft.Json;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.MVC.Services.Interfaces;
using NextERP.Util;
using System.Text;

namespace NextERP.MVC.Services.Services
{
    public class AccountAPIService : BaseAPIService, IAccountAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<string>> Auth(UserModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Tạo client không có Bearer Token
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[Constants.BaseAddress]);
            var response = await client.PostAsync("/api/Account/Authentication", httpContent);

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<APISuccessResult<string>>(await response.Content.ReadAsStringAsync())!;
            return JsonConvert.DeserializeObject<APISuccessResult<string>>(await response.Content.ReadAsStringAsync())!;
        }
    }
}
