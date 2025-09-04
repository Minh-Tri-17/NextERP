using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class ScheduleAPIService : BaseAPIService, IScheduleAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public ScheduleAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(ScheduleModel request)
        {
            return await PostAsync<APIBaseResult<bool>, ScheduleModel>(Constants.UrlCreateOrEditSchedule, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteSchedule}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlySchedule}?ids={ids}");
        }

        public async Task<APIBaseResult<ScheduleModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<ScheduleModel>>($"{Constants.UrlGetSchedule}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<ScheduleModel>>> GetPaging(ScheduleModel request)
        {
            return await PostAsync<APIBaseResult<PagingResult<ScheduleModel>>, ScheduleModel>($"{Constants.UrlGetSchedules}/{Constants.Filter}", request);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportSchedule, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(ScheduleModel request)
        {
            return await ExportAsync<APIBaseResult<byte[]>, ScheduleModel>(Constants.UrlExportSchedule, request);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
