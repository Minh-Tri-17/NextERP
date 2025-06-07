using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class RoleAPIService : BaseAPIService, IRoleAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public RoleAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(RoleModel request)
        {
            return await PostAsync<APIBaseResult<bool>, RoleModel>(Constants.UrlCreateOrEditRole, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteRole}?ids={ids}");
        }

        public async Task<APIBaseResult<RoleModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<RoleModel>>($"{Constants.UrlGetRole}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<RoleModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<RoleModel>>, Filter>($"{Constants.UrlGetRoles}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportRole, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportRole, filter);
        }
    }
}
