using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
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
            return await PostAsync<APIBaseResult<bool>, FeedbackModel>(Constants.UrlCreateOrEditFeedback, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteFeedback}/{ids}");
        }

        public async Task<APIBaseResult<FeedbackModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<FeedbackModel>>($"{Constants.UrlGetFeedback}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<FeedbackModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<FeedbackModel>>, Filter>($"{Constants.UrlGetFeedbacks}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportFeedback, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportFeedback, filter);
        }
    }
}
