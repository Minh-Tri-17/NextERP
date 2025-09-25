using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface IAccountAPIService
    {
        public Task<APIBaseResult<string>> Auth(UserModel request);
        public Task<APIBaseResult<bool>> Register(UserModel request);
        public Task<APIBaseResult<bool>> SendOTP(MailModel mail);
        public Task<APIBaseResult<bool>> ResetPassword(UserModel request);
    }
}
