using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IEmployeeService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(EmployeeModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<EmployeeModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<EmployeeModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
