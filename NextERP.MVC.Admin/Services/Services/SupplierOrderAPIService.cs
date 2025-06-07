using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class SupplierOrderAPIService : BaseAPIService, ISupplierOrderAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public SupplierOrderAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(SupplierOrderModel request)
        {
            return await PostAsync<APIBaseResult<bool>, SupplierOrderModel>(Constants.UrlCreateOrEditSupplierOrder, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteSupplierOrder}?ids={ids}");
        }

        public async Task<APIBaseResult<SupplierOrderModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<SupplierOrderModel>>($"{Constants.UrlGetSupplierOrder}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<SupplierOrderModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<SupplierOrderModel>>, Filter>($"{Constants.UrlGetSupplierOrderDetails}/{Constants.Filter}", filter);
        }
    }
}
