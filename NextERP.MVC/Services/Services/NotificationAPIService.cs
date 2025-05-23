using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class NotificationAPIService : BaseAPIService, INotificationAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public NotificationAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(NotificationModel request)
        {
            return await PostAsync<APIBaseResult<bool>, NotificationModel>($"/api/CreateOrEditNotification", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteNotification/{ids}");
        }

        public async Task<APIBaseResult<NotificationModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<NotificationModel>>($"/api/GetNotification/{id}");
        }

        public async Task<APIBaseResult<PagingResult<NotificationModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<NotificationModel>>, Filter>($"/api/GetNotifications/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportNotification", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportNotification", filter);
        }
    }
}
