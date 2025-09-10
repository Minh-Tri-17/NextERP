using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class PositionAPIService : BaseAPIService, IPositionAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public PositionAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(PositionModel request)
        {
            return await PostAsync<APIBaseResult<bool>, PositionModel>(Constants.UrlCreateOrEditPosition, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePosition}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyPosition}?ids={ids}");
        }

        public async Task<APIBaseResult<PositionModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<PositionModel>>($"{Constants.UrlGetPosition}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<PositionModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<PositionModel>>, Filter>($"{Constants.UrlGetPositions}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportPosition, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportPosition, filter);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
