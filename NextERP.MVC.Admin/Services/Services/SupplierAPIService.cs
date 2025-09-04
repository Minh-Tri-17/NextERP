using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class SupplierAPIService : BaseAPIService, ISupplierAPIService
    {
        #region Infrastructure

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

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(SupplierModel request)
        {
            return await PostAsync<APIBaseResult<bool>, SupplierModel>(Constants.UrlCreateOrEditSupplier, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteSupplier}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlySupplier}?ids={ids}");
        }

        public async Task<APIBaseResult<SupplierModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<SupplierModel>>($"{Constants.UrlGetSupplier}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<SupplierModel>>> GetPaging(SupplierModel request)
        {
            return await PostAsync<APIBaseResult<PagingResult<SupplierModel>>, SupplierModel>($"{Constants.UrlGetSuppliers}/{Constants.Filter}", request);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportSupplier, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(SupplierModel request)
        {
            return await ExportAsync<APIBaseResult<byte[]>, SupplierModel>(Constants.UrlExportSupplier, request);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
