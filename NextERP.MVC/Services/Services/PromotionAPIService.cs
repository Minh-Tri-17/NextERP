using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class PromotionAPIService : BaseAPIService, IPromotionAPIService
    {
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

        public async Task<APIBaseResult<bool>> CreateOrEdit(PromotionModel request)
        {
            return await PostAsync<APIBaseResult<bool>, PromotionModel>($"/api/CreateOrEditPromotion", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeletePromotion/{ids}");
        }

        public async Task<APIBaseResult<PromotionModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<PromotionModel>>($"/api/GetPromotion/{id}");
        }

        public async Task<APIBaseResult<PagingResult<PromotionModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<PromotionModel>>, Filter>($"/api/GetPromotions/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportPromotion", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportPromotion", filter);
        }
    }
}
