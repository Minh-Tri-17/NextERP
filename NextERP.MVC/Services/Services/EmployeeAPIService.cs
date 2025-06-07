using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
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
            return await PostAsync<APIBaseResult<bool>, EmployeeModel>(Constants.UrlCreateOrEditEmployee, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteEmployee}/{ids}");
        }

        public async Task<APIBaseResult<EmployeeModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<EmployeeModel>>($"{Constants.UrlGetEmployee}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<EmployeeModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<EmployeeModel>>, Filter>($"{Constants.UrlGetEmployees}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportEmployee, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportEmployee, filter);
        }
    }
}
