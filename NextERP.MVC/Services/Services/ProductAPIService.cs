using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class ProductAPIService : BaseAPIService, IProductAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProductAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(ProductModel request)
        {
            return await PostAsync<APIBaseResult<bool>, ProductModel>($"/api/CreateOrEditProduct", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteProduct/{ids}");
        }

        public async Task<APIBaseResult<ProductModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<ProductModel>>($"/api/GetProduct/{id}");
        }

        public async Task<APIBaseResult<PagingResult<ProductModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<ProductModel>>, Filter>($"/api/GetProducts/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportProduct", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportProduct", filter);
        }
    }
}
