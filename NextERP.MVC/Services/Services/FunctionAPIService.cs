using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class FunctionAPIService : BaseAPIService, IFunctionAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public FunctionAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(FunctionModel request)
        {
            return await PostAsync<APIBaseResult<bool>, FunctionModel>($"/api/CreateOrEditFunction", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteFunction/{ids}");
        }

        public async Task<APIBaseResult<FunctionModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<FunctionModel>>($"/api/GetFunction/{id}");
        }

        public async Task<APIBaseResult<PagingResult<FunctionModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<FunctionModel>>, Filter>($"/api/GetFunctions/Filter", filter);
        }
    }
}
