using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class PromotionAPIService : BaseAPIService, IPromotionAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public PromotionAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(PromotionModel request)
        {
            return await PostAsync<APIBaseResult<bool>, PromotionModel>(Constants.UrlCreateOrEditPromotion, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePromotion}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyPromotion}?ids={ids}");
        }

        public async Task<APIBaseResult<PromotionModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<PromotionModel>>($"{Constants.UrlGetPromotion}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<PromotionModel>>> GetPaging(PromotionModel request)
        {
            return await PostAsync<APIBaseResult<PagingResult<PromotionModel>>, PromotionModel>($"{Constants.UrlGetPromotions}/{Constants.Filter}", request);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportPromotion, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(PromotionModel request)
        {
            return await ExportAsync<APIBaseResult<byte[]>, PromotionModel>(Constants.UrlExportPromotion, request);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
