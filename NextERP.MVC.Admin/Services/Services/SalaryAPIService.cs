using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class SalaryAPIService : BaseAPIService, ISalaryAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public SalaryAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(SalaryModel request)
        {
            return await PostAsync<APIBaseResult<bool>, SalaryModel>(Constants.UrlCreateOrEditSalary, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteSalary}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlySalary}?ids={ids}");
        }

        public async Task<APIBaseResult<SalaryModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<SalaryModel>>($"{Constants.UrlGetSalary}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<SalaryModel>>> GetPaging(FilterModel filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<SalaryModel>>, FilterModel>($"{Constants.UrlGetSalaries}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportSalary, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, FilterModel>(Constants.UrlExportSalary, filter);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
