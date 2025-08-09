using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface ISupplierOrderDetailAPIService
    {
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<SupplierOrderDetailModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<SupplierOrderDetailModel>>> GetPaging(Filter filter);
    }
}
