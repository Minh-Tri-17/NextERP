using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface ISupplierOrderAPIService
    {
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<SupplierOrderModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<SupplierOrderModel>>> GetPaging(FilterModel filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(FilterModel filter);
        public Task<byte[]> GetImageBytes(Guid? supplierOrderId, string imagePath);
        public Task<APIBaseResult<bool>> Signature(SupplierOrderModel request);
    }
}
