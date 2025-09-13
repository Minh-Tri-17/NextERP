using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class HistoryMailAPIService : BaseAPIService, IHistoryMailAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public HistoryMailAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(HistoryMailModel request)
        {
            return await PostAsync<APIBaseResult<bool>, HistoryMailModel>(Constants.UrlCreateOrEditHistoryMail, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteHistoryMail}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyHistoryMail}?ids={ids}");
        }

        public async Task<APIBaseResult<HistoryMailModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<HistoryMailModel>>($"{Constants.UrlGetHistoryMail}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<HistoryMailModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<HistoryMailModel>>, Filter>($"{Constants.UrlGetHistoryMails}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportHistoryMail, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportHistoryMail, filter);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
