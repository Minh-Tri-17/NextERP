using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class HistoryNotificationAPIService : BaseAPIService, IHistoryNotificationAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public HistoryNotificationAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(HistoryNotificationModel request)
        {
            return await PostAsync<APIBaseResult<bool>, HistoryNotificationModel>(Constants.UrlCreateOrEditHistoryNotification, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteHistoryNotification}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyHistoryNotification}?ids={ids}");
        }

        public async Task<APIBaseResult<HistoryNotificationModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<HistoryNotificationModel>>($"{Constants.UrlGetHistoryNotification}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<HistoryNotificationModel>>> GetPaging(FilterModel filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<HistoryNotificationModel>>, FilterModel>($"{Constants.UrlGetHistoryNotifications}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportHistoryNotification, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, FilterModel>(Constants.UrlExportHistoryNotification, filter);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
