using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface ISupplierOrderDetailService
    {
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<SupplierOrderDetailModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<SupplierOrderDetailModel>>> GetPaging(FilterModel filter);
    }
}
