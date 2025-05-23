using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class FeedbackAPIService : BaseAPIService, IFeedbackAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public FeedbackAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(FeedbackModel request)
        {
            return await PostAsync<APIBaseResult<bool>, FeedbackModel>($"/api/CreateOrEditFeedback", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteFeedback/{ids}");
        }

        public async Task<APIBaseResult<FeedbackModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<FeedbackModel>>($"/api/GetFeedback/{id}");
        }

        public async Task<APIBaseResult<PagingResult<FeedbackModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<FeedbackModel>>, Filter>($"/api/GetFeedbacks/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportFeedback", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportFeedback", filter);
        }
    }
}
