using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class PositionAPIService : BaseAPIService, IPositionAPIService
    {
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

        public async Task<APIBaseResult<bool>> CreateOrEdit(PositionModel request)
        {
            return await PostAsync<APIBaseResult<bool>, PositionModel>($"/api/CreateOrEditPosition", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeletePosition/{ids}");
        }

        public async Task<APIBaseResult<PositionModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<PositionModel>>($"/api/GetPosition/{id}");
        }

        public async Task<APIBaseResult<PagingResult<PositionModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<PositionModel>>, Filter>($"/api/GetPositions/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportPosition", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportPosition", filter);
        }
    }
}
