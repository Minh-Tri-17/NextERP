using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class TrainingSessionAPIService : BaseAPIService, ITrainingSessionAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public TrainingSessionAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(TrainingSessionModel request)
        {
            return await PostAsync<APIBaseResult<bool>, TrainingSessionModel>($"/api/CreateOrEditTrainingSession", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteTrainingSession/{ids}");
        }

        public async Task<APIBaseResult<TrainingSessionModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<TrainingSessionModel>>($"/api/GetTrainingSession/{id}");
        }

        public async Task<APIBaseResult<PagingResult<TrainingSessionModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<TrainingSessionModel>>, Filter>($"/api/GetTrainingSessions/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportTrainingSession", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportTrainingSession", filter);
        }
    }
}
