using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class DepartmentAPIService : BaseAPIService, IDepartmentAPIService
    {
        #region Infrastructure

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

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(DepartmentModel request)
        {
            return await PostAsync<APIBaseResult<bool>, DepartmentModel>(Constants.UrlCreateOrEditDepartment, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteDepartment}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyDepartment}?ids={ids}");
        }

        public async Task<APIBaseResult<DepartmentModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<DepartmentModel>>($"{Constants.UrlGetDepartment}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<DepartmentModel>>> GetPaging(DepartmentModel request)
        {
            return await PostAsync<APIBaseResult<PagingResult<DepartmentModel>>, DepartmentModel>($"{Constants.UrlGetDepartments}/{Constants.Filter}", request);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportDepartment, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(DepartmentModel request)
        {
            return await ExportAsync<APIBaseResult<byte[]>, DepartmentModel>(Constants.UrlExportDepartment, request);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
