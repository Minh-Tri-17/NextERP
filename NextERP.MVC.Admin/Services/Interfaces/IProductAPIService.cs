using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface IProductAPIService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(ProductModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<ProductModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<ProductModel>>> GetPaging(ProductModel request);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(ProductModel request);
        public Task<byte[]> GetImageBytes(Guid? productId, string imagePath);
    }
}
