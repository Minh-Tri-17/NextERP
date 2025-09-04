using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IDepartmentService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(DepartmentModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<DepartmentModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<DepartmentModel>>> GetPaging(DepartmentModel request);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(DepartmentModel request);
    }
}
