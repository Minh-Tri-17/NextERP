using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class ProductAPIService : BaseAPIService, IProductAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProductAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(ProductModel request)
        {
            return await PostAsync<APIBaseResult<bool>, ProductModel>(Constants.UrlCreateOrEditProduct, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteProduct}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyProduct}?ids={ids}");
        }

        public async Task<APIBaseResult<ProductModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<ProductModel>>($"{Constants.UrlGetProduct}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<ProductModel>>> GetPaging(FilterModel filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<ProductModel>>, FilterModel>($"{Constants.UrlGetProducts}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportProduct, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, FilterModel>(Constants.UrlExportProduct, filter);
        }

        #endregion

        #region Custom Operations

        public async Task<byte[]> GetImageBytes(Guid? productId, string imagePath)
        {
            return await GetAsync<byte[]>(string.Format(Constants.UrlGetImageProduct, productId, imagePath));
        }

        #endregion
    }
}
