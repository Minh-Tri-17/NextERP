using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class TrainingSessionAPIService : BaseAPIService, ITrainingSessionAPIService
    {
        #region Infrastructure

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

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(TrainingSessionModel request)
        {
            return await PostAsync<APIBaseResult<bool>, TrainingSessionModel>(Constants.UrlCreateOrEditTrainingSession, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteTrainingSession}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyTrainingSession}?ids={ids}");
        }

        public async Task<APIBaseResult<TrainingSessionModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<TrainingSessionModel>>($"{Constants.UrlGetTrainingSession}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<TrainingSessionModel>>> GetPaging(TrainingSessionModel request)
        {
            return await PostAsync<APIBaseResult<PagingResult<TrainingSessionModel>>, TrainingSessionModel>($"{Constants.UrlGetTrainingSessions}/{Constants.Filter}", request);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportTrainingSession, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(TrainingSessionModel request)
        {
            return await ExportAsync<APIBaseResult<byte[]>, TrainingSessionModel>(Constants.UrlExportTrainingSession, request);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
