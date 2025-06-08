using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IRoleService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(RoleModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<RoleModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<RoleModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
