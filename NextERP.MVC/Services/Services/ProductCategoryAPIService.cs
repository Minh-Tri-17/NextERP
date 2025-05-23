using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
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
            return await PostAsync<APIBaseResult<bool>, ProductCategoryModel>($"/api/CreateOrEditProductCategory", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteProductCategory/{ids}");
        }

        public async Task<APIBaseResult<ProductCategoryModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<ProductCategoryModel>>($"/api/GetProductCategory/{id}");
        }

        public async Task<APIBaseResult<PagingResult<ProductCategoryModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<ProductCategoryModel>>, Filter>($"/api/GetProductCategorys/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportProductCategory", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportProductCategory", filter);
        }
    }
}
