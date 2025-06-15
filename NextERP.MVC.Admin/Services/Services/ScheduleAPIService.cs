using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class ScheduleAPIService : BaseAPIService, IScheduleAPIService
    {
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

        public async Task<APIBaseResult<PagingResult<ScheduleModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<ScheduleModel>>, Filter>($"{Constants.UrlGetSchedules}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportSchedule, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportSchedule, filter);
        }
    }
}
