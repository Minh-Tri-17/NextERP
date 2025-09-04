using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IProductCategoryService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(ProductCategoryModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<ProductCategoryModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<ProductCategoryModel>>> GetPaging(ProductCategoryModel request);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(ProductCategoryModel request);
    }
}
