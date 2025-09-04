using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface ISpaServiceCategoryService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(SpaServiceCategoryModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<SpaServiceCategoryModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<SpaServiceCategoryModel>>> GetPaging(SpaServiceCategoryModel request);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(SpaServiceCategoryModel request);
    }
}
