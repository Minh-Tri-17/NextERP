﻿using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface ISupplierService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(Guid id, SupplierModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<SupplierModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<SupplierModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
