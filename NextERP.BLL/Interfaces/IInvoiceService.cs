using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IInvoiceService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(InvoiceModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<InvoiceModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<InvoiceModel>>> GetPaging(FilterModel filter);
    }
}
