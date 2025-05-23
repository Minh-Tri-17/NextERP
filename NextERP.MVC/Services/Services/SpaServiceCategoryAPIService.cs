using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class SpaServiceCategoryAPIService : BaseAPIService, ISpaServiceCategoryAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public SpaServiceCategoryAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(SpaServiceCategoryModel request)
        {
            return await PostAsync<APIBaseResult<bool>, SpaServiceCategoryModel>($"/api/CreateOrEditSpaServiceCategory", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteSpaServiceCategory/{ids}");
        }

        public async Task<APIBaseResult<SpaServiceCategoryModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<SpaServiceCategoryModel>>($"/api/GetSpaServiceCategory/{id}");
        }

        public async Task<APIBaseResult<PagingResult<SpaServiceCategoryModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<SpaServiceCategoryModel>>, Filter>($"/api/GetSpaServiceCategorys/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportSpaServiceCategory", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportSpaServiceCategory", filter);
        }
    }
}
