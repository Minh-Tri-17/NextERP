using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IProductCategoryService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(Guid id, ProductCategoryModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<ProductCategoryModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<ProductCategoryModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
