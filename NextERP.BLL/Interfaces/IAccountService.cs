using Microsoft.AspNetCore.Identity;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;

namespace NextERP.BLL.Interface
{
    public interface IAccountService
    {
        public Task<APIBaseResult<string>> Auth(UserModel request);
        public Task<APIBaseResult<bool>> Register(UserModel request);
        public Task<APIBaseResult<bool>> SendOTP(MailModel mail);
        public Task<APIBaseResult<bool>> ResetPassword(UserModel request);
        public List<IdentityError> ValidatePassword(string password);
    }
}
