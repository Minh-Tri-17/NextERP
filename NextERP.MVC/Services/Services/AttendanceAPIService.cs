﻿using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Services.Services
{
    public class AttendanceAPIService : BaseAPIService, IAttendanceAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public AttendanceAPIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IConfiguration configuration)
            : base(httpClientFactory, contextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(AttendanceModel request)
        {
            return await PostAsync<APIBaseResult<bool>, AttendanceModel>(Constants.UrlCreateOrEditAttendance, request);
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            return await DeleteAsync<APIBaseResult<bool>>($"{Constants.UrlCreateOrEditAttendance}/{ids}");
        }

        public async Task<APIBaseResult<AttendanceModel>> GetOne(Guid id)
        {
            return await GetAsync<APIBaseResult<AttendanceModel>>($"{Constants.UrlGetAttendance}/{id}");
        }

        public async Task<APIBaseResult<PagingResult<AttendanceModel>>> GetPaging(Filter filter)
        {
            return await PostAsync<APIBaseResult<PagingResult<AttendanceModel>>, Filter>($"{Constants.UrlGetAttendances}/{Constants.Filter}", filter);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            return await ImportAsync<APIBaseResult<bool>>(Constants.UrlImportAttendance, fileImport);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            return await ExportAsync<APIBaseResult<byte[]>, Filter>(Constants.UrlExportAttendance, filter);
        }
    }
}
