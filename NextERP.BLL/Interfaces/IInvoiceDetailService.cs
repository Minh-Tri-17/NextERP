﻿using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IInvoiceDetailService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(Guid id, InvoiceDetailModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<InvoiceDetailModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<InvoiceDetailModel>>> GetPaging(Filter filter);
    }
}
