using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class AppointmentAPIService : BaseAPIService, IAppointmentAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public AppointmentAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(AppointmentModel request)
        {
            return await PostAsync<APIBaseResult<bool>, AppointmentModel>(Constants.UrlCreateOrEditAppointment, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteAppointment}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyAppointment}?ids={ids}");
        }

        public async Task<APIBaseResult<AppointmentModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<AppointmentModel>>($"{Constants.UrlGetAppointment}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<AppointmentModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<AppointmentModel>>, Filter>($"{Constants.UrlGetAppointments}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportAppointment, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportAppointment, filter);
        }
    }
}
