using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class BranchAPIService : BaseAPIService, IBranchAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public BranchAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(BranchModel request)
        {
            return await PostAsync<APIBaseResult<bool>, BranchModel>($"/api/CreateOrEditBranch", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteBranch/{ids}");
        }

        public async Task<APIBaseResult<BranchModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<BranchModel>>($"/api/GetBranch/{id}");
        }

        public async Task<APIBaseResult<PagingResult<BranchModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<BranchModel>>, Filter>($"/api/GetBranchs/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportBranch", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportBranch", filter);
        }
    }
}
