﻿using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IFeedbackService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(FeedbackModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<FeedbackModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<FeedbackModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
