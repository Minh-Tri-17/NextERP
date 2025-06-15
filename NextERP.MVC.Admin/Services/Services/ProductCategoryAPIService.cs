using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class ProductCategoryAPIService : BaseAPIService, IProductCategoryAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProductCategoryAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(ProductCategoryModel request)
        {
            return await PostAsync<APIBaseResult<bool>, ProductCategoryModel>(Constants.UrlCreateOrEditProductCategory, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteProductCategory}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyProductCategory}?ids={ids}");
        }

        public async Task<APIBaseResult<ProductCategoryModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<ProductCategoryModel>>($"{Constants.UrlGetProductCategory}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<ProductCategoryModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<ProductCategoryModel>>, Filter>($"{Constants.UrlGetProductCategories}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportProductCategory, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportProductCategory, filter);
        }
    }
}
