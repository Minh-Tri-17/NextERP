using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
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
            return await PostAsync<APIBaseResult<bool>, InvoiceDetailModel>(Constants.UrlCreateOrEditInvoiceDetail, request);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteInvoiceDetail}?ids={ids}");
        }

        public async Task<APIBaseResult<InvoiceDetailModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<InvoiceDetailModel>>($"{Constants.UrlGetInvoiceDetail}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<InvoiceDetailModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<InvoiceDetailModel>>, Filter>($"{Constants.UrlGetInvoiceDetails}/{Constants.Filter}", filter);
        }
    }
}
