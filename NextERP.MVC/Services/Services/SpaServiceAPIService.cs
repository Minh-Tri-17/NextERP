using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class SpaServiceAPIService : BaseAPIService, ISpaServiceAPIService
    {
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

        public async Task<APIBaseResult<bool>> CreateOrEdit(SpaServiceModel request)
        {
            return await PostAsync<APIBaseResult<bool>, SpaServiceModel>(Constants.UrlCreateOrEditSpaService, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteSpaService}/{ids}");
        }

        public async Task<APIBaseResult<SpaServiceModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<SpaServiceModel>>($"{Constants.UrlGetSpaService}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<SpaServiceModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<SpaServiceModel>>, Filter>($"{Constants.UrlGetSpaServices}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportSpaService, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportSpaService, filter);
        }
    }
}
