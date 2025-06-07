using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IUserService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(Guid id, UserModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<UserModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<UserModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
