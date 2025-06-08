using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IPromotionService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(PromotionModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<PromotionModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<PromotionModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
