using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class LeaveRequestAPIService : BaseAPIService, ILeaveRequestAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public LeaveRequestAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(LeaveRequestModel request)
        {
            return await PostAsync<APIBaseResult<bool>, LeaveRequestModel>(Constants.UrlCreateOrEditLeaveRequest, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteLeaveRequest}?ids={ids}");
        }

        public async Task<APIBaseResult<LeaveRequestModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<LeaveRequestModel>>($"{Constants.UrlGetLeaveRequest}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<LeaveRequestModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<LeaveRequestModel>>, Filter>($"{Constants.UrlGetLeaveRequests}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportLeaveRequest, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportLeaveRequest, filter);
        }
    }
}
