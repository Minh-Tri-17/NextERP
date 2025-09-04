using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class InvoiceAPIService : BaseAPIService, IInvoiceAPIService
    {
        #region Infrastructure

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

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(InvoiceModel request)
        {
            return await PostAsync<APIBaseResult<bool>, InvoiceModel>(Constants.UrlCreateOrEditInvoice, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteInvoice}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyInvoice}?ids={ids}");
        }

        public async Task<APIBaseResult<InvoiceModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<InvoiceModel>>($"{Constants.UrlGetInvoice}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<InvoiceModel>>> GetPaging(InvoiceModel request)
        {
            return await PostAsync<APIBaseResult<PagingResult<InvoiceModel>>, InvoiceModel>($"{Constants.UrlGetInvoices}/{Constants.Filter}", request);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
