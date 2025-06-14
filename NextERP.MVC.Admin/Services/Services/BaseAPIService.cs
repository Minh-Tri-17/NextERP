using Newtonsoft.Json;
using NextERP.ModelBase.APIResult;
using NextERP.Util;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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
            var contentType = response.Content.Headers.ContentType?.MediaType;

            // Nếu kiểu dữ liệu trả về là byte[] hoặc là file/hình ảnh
            if (typeof(TResponse) == typeof(byte[]) || (contentType != null && contentType.StartsWith("image/")))
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                return (TResponse)(object)bytes!;
            }

            // Nếu kiểu dữ liệu trả về là APIBaseResult<byte[]>
            if (typeof(TResponse) == typeof(APIBaseResult<byte[]>))
            {
                var contentString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<APIBaseResult<byte[]>>(contentString);
                return (TResponse)(object)result!;
            }

            // Mặc định: đọc chuỗi JSON
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

            // Lấy tất cả các file từ các thuộc tính của request
            var fileProps = typeof(TRequest).GetProperties()
                .Where(p => typeof(IFormFile).IsAssignableFrom(p.PropertyType)
                || typeof(IEnumerable<IFormFile>).IsAssignableFrom(p.PropertyType))
                .ToList();

            var files = fileProps.SelectMany(s =>
               {
                   var value = s.GetValue(request);
                   return value switch
                   {
                       IFormFile f => new[] { f },
                       IEnumerable<IFormFile> fs => fs,
                       _ => Enumerable.Empty<IFormFile>()
                   };
               }).ToList();

            HttpContent content;

            // Nếu có file đính kèm, sử dụng MultipartFormDataContent để gửi dữ liệu và file ngược lại sử dụng json đơn giản
            if (files.Any())
            {
                var multipartContent = new MultipartFormDataContent();

                // Thêm các thuộc tính đơn giản (không phải file) của request vào multipartContent
                var jsonProps = typeof(TRequest).GetProperties()
                    .Where(p => !fileProps.Contains(p))
                    .ToDictionary(p => p.Name, p => p.GetValue(request));

                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(jsonProps), Encoding.UTF8), "Json");

                // Thêm các file đính kèm vào multipartContent
                foreach (var file in files)
                {
                    var streamContent = new StreamContent(file.OpenReadStream());
                    streamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = Constants.Files,
                        FileName = $"\"{file.FileName}\""
                    };
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                    multipartContent.Add(streamContent, file.Name, file.FileName);
                }

                content = multipartContent;
            }
            else
            {
                content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            }

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
