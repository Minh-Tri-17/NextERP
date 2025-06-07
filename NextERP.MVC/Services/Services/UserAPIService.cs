using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class UserAPIService : BaseAPIService, IUserAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(UserModel request)
        {
            return await PostAsync<APIBaseResult<bool>, UserModel>(Constants.UrlCreateOrEditUser, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteUser}/{ids}");
        }

        public async Task<APIBaseResult<UserModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<UserModel>>($"{Constants.UrlGetUser}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<UserModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<UserModel>>, Filter>($"{Constants.UrlGetUsers}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportUser, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportUser, filter);
        }
    }
}
