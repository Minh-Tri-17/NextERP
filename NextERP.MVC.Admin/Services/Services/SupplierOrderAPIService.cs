using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class SupplierOrderAPIService : BaseAPIService, ISupplierOrderAPIService
    {
        #region Infrastructure

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

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteSupplierOrder}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlySupplierOrder}?ids={ids}");
        }

        public async Task<APIBaseResult<SupplierOrderModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<SupplierOrderModel>>($"{Constants.UrlGetSupplierOrder}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<SupplierOrderModel>>> GetPaging(FilterModel filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<SupplierOrderModel>>, FilterModel>($"{Constants.UrlGetSupplierOrders}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportSupplierOrder, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, FilterModel>(Constants.UrlExportSupplierOrder, filter);
        }

        #endregion

        #region Custom Operations

        public async Task<byte[]> GetImageBytes(Guid? supplierOrderId, string imagePath)
        {
            return await GetAsync<byte[]>(string.Format(Constants.UrlGetImageSupplierOrder, supplierOrderId, imagePath));
        }

        public async Task<APIBaseResult<bool>> Signature(SupplierOrderModel request)
        {
            return await PostAsync<APIBaseResult<bool>, SupplierOrderModel>(Constants.UrlSignatureSupplierOrder, request);
        }

        #endregion
    }
}
