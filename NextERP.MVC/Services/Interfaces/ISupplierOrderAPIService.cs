using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface ISupplierOrderAPIService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(SupplierOrderModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<SupplierOrderModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<SupplierOrderModel>>> GetPaging(Filter filter);
    }
}
