using Newtonsoft.Json;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;
using System.Text;

namespace NextERP.MVC.Admin.Services.Services
{
    public class AccountAPIService : BaseAPIService, IAccountAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration) : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        #endregion

        #region Custom Operations

        public async Task<APIBaseResult<string>> Auth(UserModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var baseAddress = _configuration[Constants.APIAddress];
            // Tạo client không có Bearer Token
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseAddress);
            var response = await client.PostAsync(Constants.UrlAuthentication, httpContent);

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<APISuccessResult<string>>(await response.Content.ReadAsStringAsync())!;

            return JsonConvert.DeserializeObject<APIErrorResult<string>>(await response.Content.ReadAsStringAsync())!;
        }

        public async Task<APIBaseResult<bool>> Register(UserModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var baseAddress = _configuration[Constants.APIAddress];
            // Tạo client không có Bearer Token
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseAddress);
            var response = await client.PostAsync(Constants.UrlRegister, httpContent);

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<APISuccessResult<bool>>(await response.Content.ReadAsStringAsync())!;

            return JsonConvert.DeserializeObject<APIErrorResult<bool>>(await response.Content.ReadAsStringAsync())!;
        }

        public async Task<APIBaseResult<bool>> SendOTP(MailModel mail)
        {
            var json = JsonConvert.SerializeObject(mail);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var baseAddress = _configuration[Constants.APIAddress];
            // Tạo client không có Bearer Token
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseAddress);
            var response = await client.PostAsync(Constants.UrlSendOTP, httpContent);

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<APISuccessResult<bool>>(await response.Content.ReadAsStringAsync())!;

            return JsonConvert.DeserializeObject<APIErrorResult<bool>>(await response.Content.ReadAsStringAsync())!;
        }

        #endregion
    }
}
