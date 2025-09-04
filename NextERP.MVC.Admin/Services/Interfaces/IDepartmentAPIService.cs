using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface IDepartmentAPIService
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
