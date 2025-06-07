using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class CustomerAPIService : BaseAPIService, ICustomerAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomerAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(CustomerModel request)
        {
            return await PostAsync<APIBaseResult<bool>, CustomerModel>(Constants.UrlCreateOrEditCustomer, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteCustomer}?ids={ids}");
        }

        public async Task<APIBaseResult<CustomerModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<CustomerModel>>($"{Constants.UrlGetCustomer}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<CustomerModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<CustomerModel>>, Filter>($"{Constants.UrlGetCustomers}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportCustomer, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportCustomer, filter);
        }
    }
}
