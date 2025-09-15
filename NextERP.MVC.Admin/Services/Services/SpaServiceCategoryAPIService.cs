using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class SpaServiceCategoryAPIService : BaseAPIService, ISpaServiceCategoryAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public SpaServiceCategoryAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(SpaServiceCategoryModel request)
        {
            return await PostAsync<APIBaseResult<bool>, SpaServiceCategoryModel>(Constants.UrlCreateOrEditSpaServiceCategory, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteSpaServiceCategory}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlySpaServiceCategory}?ids={ids}");
        }

        public async Task<APIBaseResult<SpaServiceCategoryModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<SpaServiceCategoryModel>>($"{Constants.UrlGetSpaServiceCategory}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<SpaServiceCategoryModel>>> GetPaging(FilterModel filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<SpaServiceCategoryModel>>, FilterModel>($"{Constants.UrlGetSpaServiceCategories}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportSpaServiceCategory, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, FilterModel>(Constants.UrlExportSpaServiceCategory, filter);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
