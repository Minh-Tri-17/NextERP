using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class TemplateNotificationAPIService : BaseAPIService, ITemplateNotificationAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public TemplateNotificationAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(TemplateNotificationModel request)
        {
            return await PostAsync<APIBaseResult<bool>, TemplateNotificationModel>(Constants.UrlCreateOrEditTemplateNotification, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteTemplateNotification}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyTemplateNotification}?ids={ids}");
        }

        public async Task<APIBaseResult<TemplateNotificationModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<TemplateNotificationModel>>($"{Constants.UrlGetTemplateNotification}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<TemplateNotificationModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<TemplateNotificationModel>>, Filter>($"{Constants.UrlGetTemplateNotifications}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportTemplateNotification, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportTemplateNotification, filter);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
