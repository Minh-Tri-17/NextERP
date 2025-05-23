using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class SupplierAPIService : BaseAPIService, ISupplierAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public SupplierAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(SupplierModel request)
        {
            return await PostAsync<APIBaseResult<bool>, SupplierModel>($"/api/CreateOrEditSupplier", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteSupplier/{ids}");
        }

        public async Task<APIBaseResult<SupplierModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<SupplierModel>>($"/api/GetSupplier/{id}");
        }

        public async Task<APIBaseResult<PagingResult<SupplierModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<SupplierModel>>, Filter>($"/api/GetSuppliers/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportSupplier", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportSupplier", filter);
        }
    }
}
