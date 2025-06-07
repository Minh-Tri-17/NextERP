using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface ICustomerAPIService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(CustomerModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<CustomerModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<CustomerModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
