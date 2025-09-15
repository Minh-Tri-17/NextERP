using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class SpaServiceAPIService : BaseAPIService, ISpaServiceAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public SpaServiceAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(SpaServiceModel request)
        {
            return await PostAsync<APIBaseResult<bool>, SpaServiceModel>(Constants.UrlCreateOrEditSpaService, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteSpaService}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlySpaService}?ids={ids}");
        }

        public async Task<APIBaseResult<SpaServiceModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<SpaServiceModel>>($"{Constants.UrlGetSpaService}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<SpaServiceModel>>> GetPaging(FilterModel filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<SpaServiceModel>>, FilterModel>($"{Constants.UrlGetSpaServices}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportSpaService, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, FilterModel>(Constants.UrlExportSpaService, filter);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
