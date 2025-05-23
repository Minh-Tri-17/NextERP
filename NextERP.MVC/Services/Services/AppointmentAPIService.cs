using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
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
            return await PostAsync<APIBaseResult<bool>, AppointmentModel>($"/api/CreateOrEditAppointment", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteAppointment/{ids}");
        }

        public async Task<APIBaseResult<AppointmentModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<AppointmentModel>>($"/api/GetAppointment/{id}");
        }

        public async Task<APIBaseResult<PagingResult<AppointmentModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<AppointmentModel>>, Filter>($"/api/GetAppointments/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportAppointment", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportAppointment", filter);
        }
    }
}
