using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Services.Interfaces;

namespace NextERP.MVC.Services.Services
{
    public class DepartmentAPIService : BaseAPIService, IDepartmentAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public DepartmentAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(DepartmentModel request)
        {
            return await PostAsync<APIBaseResult<bool>, DepartmentModel>($"/api/CreateOrEditDepartment", request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"/api/DeleteDepartment/{ids}");
        }

        public async Task<APIBaseResult<DepartmentModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<DepartmentModel>>($"/api/GetDepartment/{id}");
        }

        public async Task<APIBaseResult<PagingResult<DepartmentModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<DepartmentModel>>, Filter>($"/api/GetDepartments/Filter", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>($"/api/ImportDepartment", fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>($"/api/ExportDepartment", filter);
        }
    }
}
