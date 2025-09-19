using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class TemplateMailAPIService : BaseAPIService, ITemplateMailAPIService
    {
        #region Infrastructure

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public TemplateMailAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(TemplateMailModel request)
        {
            return await PostAsync<APIBaseResult<bool>, TemplateMailModel>(Constants.UrlCreateOrEditTemplateMail, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeleteTemplateMail}?ids={ids}");
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlDeletePermanentlyTemplateMail}?ids={ids}");
        }

        public async Task<APIBaseResult<TemplateMailModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<TemplateMailModel>>($"{Constants.UrlGetTemplateMail}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<TemplateMailModel>>> GetPaging(FilterModel filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<TemplateMailModel>>, FilterModel>($"{Constants.UrlGetTemplateMails}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportTemplateMail, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, FilterModel>(Constants.UrlExportTemplateMail, filter);
        }

        #endregion

        #region Custom Operations

        public async Task<APIBaseResult<bool>> SendMail(MailModel mail)
        {
            return await PostAsync<APIBaseResult<bool>, MailModel>(Constants.UrlSendMailTemplateMail, mail);
        }

        #endregion
    }
}
