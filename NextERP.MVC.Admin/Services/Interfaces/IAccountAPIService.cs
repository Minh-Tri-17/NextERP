using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface IAccountAPIService
    {
        public Task<APIBaseResult<string>> Auth(UserModel request);
    }
}
