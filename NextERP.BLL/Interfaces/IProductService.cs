using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IProductService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(ProductModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<ProductModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<ProductModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
        public Task<byte[]> GetImageBytes(Guid productId, string imagePath);
    }
}
