using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class NotificationAPIService : BaseAPIService, INotificationAPIService
    {
        #region Infrastructure

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

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(NotificationModel request)
        {
            return await PostAsync<APIBaseResult<bool>, NotificationModel>(Constants.UrlCreateOrEditNotification, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteNotification}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyNotification}?ids={ids}");
        }

        public async Task<APIBaseResult<NotificationModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<NotificationModel>>($"{Constants.UrlGetNotification}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<NotificationModel>>> GetPaging(NotificationModel request)
        {
            return await PostAsync<APIBaseResult<PagingResult<NotificationModel>>, NotificationModel>($"{Constants.UrlGetNotifications}/{Constants.Filter}", request);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportNotification, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(NotificationModel request)
        {
            return await ExportAsync<APIBaseResult<byte[]>, NotificationModel>(Constants.UrlExportNotification, request);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
