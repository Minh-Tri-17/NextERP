using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class InvoiceAPIService : BaseAPIService, IInvoiceAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public InvoiceAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(InvoiceModel request)
        {
            return await PostAsync<APIBaseResult<bool>, InvoiceModel>($"/api/CreateOrEditInvoice", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteInvoice/{ids}");
        }

        public async Task<APIBaseResult<InvoiceModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<InvoiceModel>>($"/api/GetInvoice/{id}");
        }

        public async Task<APIBaseResult<PagingResult<InvoiceModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<InvoiceModel>>, Filter>($"/api/GetInvoices/Filter", filter);
        }
    }
}
