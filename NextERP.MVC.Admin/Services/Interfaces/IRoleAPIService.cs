using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface IRoleAPIService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(RoleModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<RoleModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<RoleModel>>> GetPaging(RoleModel request);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(RoleModel request);
    }
}
