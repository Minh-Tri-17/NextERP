using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class InvoiceDetailAPIService : BaseAPIService, IInvoiceDetailAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public InvoiceDetailAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(InvoiceDetailModel request)
        {
            return await PostAsync<APIBaseResult<bool>, InvoiceDetailModel>($"/api/CreateOrEditInvoiceDetail", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteInvoiceDetail/{ids}");
        }

        public async Task<APIBaseResult<InvoiceDetailModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<InvoiceDetailModel>>($"/api/GetInvoiceDetail/{id}");
        }

        public async Task<APIBaseResult<PagingResult<InvoiceDetailModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<InvoiceDetailModel>>, Filter>($"/api/GetInvoiceDetails/Filter", filter);
        }
    }
}
