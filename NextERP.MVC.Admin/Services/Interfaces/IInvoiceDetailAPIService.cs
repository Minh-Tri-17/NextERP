using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface IInvoiceDetailAPIService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(InvoiceDetailModel request);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<InvoiceDetailModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<InvoiceDetailModel>>> GetPaging(InvoiceDetailModel request);
    }
}
