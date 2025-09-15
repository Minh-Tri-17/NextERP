using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class FunctionAPIService : BaseAPIService, IFunctionAPIService
    {
        #region Infrastructure

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

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(FunctionModel request)
        {
            return await PostAsync<APIBaseResult<bool>, FunctionModel>(Constants.UrlCreateOrEditFunction, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteFunction}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyFunction}?ids={ids}");
        }

        public async Task<APIBaseResult<FunctionModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<FunctionModel>>($"{Constants.UrlGetFunction}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<FunctionModel>>> GetPaging(FilterModel filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<FunctionModel>>, FilterModel>($"{Constants.UrlGetFunctions}/{Constants.Filter}", filter);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
