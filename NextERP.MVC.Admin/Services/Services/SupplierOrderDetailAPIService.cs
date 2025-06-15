using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class SupplierOrderDetailAPIService : BaseAPIService, ISupplierOrderDetailAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public SupplierOrderDetailAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(SupplierOrderDetailModel request)
        {
            return await PostAsync<APIBaseResult<bool>, SupplierOrderDetailModel>(Constants.UrlCreateOrEditSupplierOrderDetail, request);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteSupplierOrderDetail}?ids={ids}");
        }

        public async Task<APIBaseResult<SupplierOrderDetailModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<SupplierOrderDetailModel>>($"{Constants.UrlGetSupplierOrderDetail}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<SupplierOrderDetailModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<SupplierOrderDetailModel>>, Filter>($"{Constants.UrlGetSupplierOrderDetails}/{Constants.Filter}", filter);
        }
    }
}
