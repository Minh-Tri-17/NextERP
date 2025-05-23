using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class EmployeeAPIService : BaseAPIService, IEmployeeAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public EmployeeAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(EmployeeModel request)
        {
            return await PostAsync<APIBaseResult<bool>, EmployeeModel>($"/api/CreateOrEditEmployee", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteEmployee/{ids}");
        }

        public async Task<APIBaseResult<EmployeeModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<EmployeeModel>>($"/api/GetEmployee/{id}");
        }

        public async Task<APIBaseResult<PagingResult<EmployeeModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<EmployeeModel>>, Filter>($"/api/GetEmployees/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportEmployee", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportEmployee", filter);
        }
    }
}
