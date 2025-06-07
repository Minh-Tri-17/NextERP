using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using NextERP.ModelBase.APIResult;
using NextERP.Util;
using System.Net.Http.Headers;
using System.Text;

namespace NextERP.MVC.Admin.Services.Services
{
    public class BaseAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        protected BaseAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Tạo một đối tượng HttpClient có kèm theo token xác thực (Bearer Token) lấy từ cookies.
        /// </summary>
        /// <returns></returns>
        private HttpClient CreateAuthorizedClient()
        {
            // Lấy token từ cookies hiện tại của người dùng
            var cookiesToken = _contextAccessor?.HttpContext?.Request.Cookies[Constants.Token];

            // Tạo mới một HttpClient thông qua IHttpClientFactory
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[Constants.BaseAddress]); // Thiết lập địa chỉ cơ sở của API từ cấu hình appsettings.json
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookiesToken); // Thiết lập Authorization header dạng Bearer Token

            return client;
        }

        /// <summary>
        /// Đọc và xử lý phản hồi từ HTTP response, tự động chuyển thành kiểu dữ liệu mong muốn.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<TResponse> ReadResponse<TResponse>(HttpResponseMessage response)
        {
            // Nếu kiểu dữ liệu trả về là mảng byte (dùng cho tải file)
            if (typeof(TResponse) == typeof(APIBaseResult<byte[]>))
            {
                var contentString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<APIBaseResult<byte[]>>(contentString);
                return (TResponse)(object)result!;
            }

            // Nếu là dữ liệu dạng JSON, đọc chuỗi nội dung
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(body)!;
        }

        protected async Task<TResponse> GetAsync<TResponse>(string url)
        {
            using var client = CreateAuthorizedClient();
            var response = await client.GetAsync(url);
            return await ReadResponse<TResponse>(response);
        }

        protected async Task<TResponse> PostAsync<TResponse, TRequest>(string url, TRequest request)
        {
            using var client = CreateAuthorizedClient();
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            return await ReadResponse<TResponse>(response);
        }

        protected async Task<TResponse> DeleteAsync<TResponse>(string url)
        {
            using var client = CreateAuthorizedClient();
            var response = await client.DeleteAsync(url);
            return await ReadResponse<TResponse>(response);
        }

        protected async Task<TResponse> ImportAsync<TResponse>(string url, IFormFile fileImport)
        {
            using var client = CreateAuthorizedClient();
            var content = new MultipartFormDataContent();

            if (fileImport != null && fileImport.Length > 0)
            {
                var streamContent = new StreamContent(fileImport.OpenReadStream());
                content.Add(streamContent, Constants.ExcelFiles, fileImport.FileName);
            }

            var response = await client.PostAsync(url, content);
            return await ReadResponse<TResponse>(response);
        }

        protected async Task<TResponse> ExportAsync<TResponse, TRequest>(string url, TRequest request)
        {
            using var client = CreateAuthorizedClient();
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            return await ReadResponse<TResponse>(response);
        }
    }
}
