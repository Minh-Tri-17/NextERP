namespace NextERP.Util
{
    public class Messages
    {
        #region Messages base

        public const string NotFoundUpdate = "NotFoundUpdate";
        public const string NotFoundGet = "NotFoundGet";
        public const string NotFoundGetList = "NotFoundGetList";
        public const string CreateSuccess = "CreateSuccess";
        public const string UpdateSuccess = "UpdateSuccess";
        public const string DeleteSuccess = "DeleteSuccess";
        public const string ImportSuccess = "ImportSuccess";
        public const string ExportSuccess = "ExportSuccess";
        public const string SendMailSuccess = "SendMailSuccess";
        public const string CreateFailed = "CreateFailed";
        public const string UpdateFailed = "UpdateFailed";
        public const string DeleteFailed = "DeleteFailed";
        public const string ImportFailed = "ImportFailed";
        public const string ExportFailed = "ExportFailed";
        public const string SendMailFailed = "SendMailFailed";
        public const string GetListResultSuccess = "GetListResultSuccess";
        public const string GetResultSuccess = "GetResultSuccess";

        #endregion

        #region Meassages auth

        public const string AuthSuccess = "AuthSuccess";
        public const string AuthFailed = "AuthFailed";
        public const string UserNameNotExist = "UserName {0} is not exist.";
        public const string UserNameExist = "UserName {0} is exist.";
        public const string UserNameOrPasswordIncorrect = "UserName {0} or Password {1} is incorrect.";
        public const string TokenKeyNotConfigured = "Token key is not configured properly in the application settings.";
        public const string RoleNotExist = "Role Customer is not exist.";

        #endregion

        public const string FileNotFound = "FileNotFound";
        public const string NotFoundSupplier = "NotFoundSupplier";
        public const string YourOTP = "YourOTP";
        public const string ResetPasswordFailed = "ResetPasswordFailed";
        public const string ResetPasswordSuccess = "ResetPasswordSuccess";
        public const string RequiredLength = "RequiredLength";
        public const string RequireDigit = "RequireDigit";
        public const string RequireLowercase = "RequireLowercase";
        public const string RequireUppercase = "RequireUppercase";
        public const string RequireNonAlphanumeric = "RequireNonAlphanumeric";
        public const string RequiredUniqueChars = "RequiredUniqueChars";
        public const string OTPNotFound = "OTPNotFound";
        public const string OTPExpired = "OTPExpired";
        public const string OTPNoAttemptsLeft = "OTPNoAttemptsLeft";
        public const string OTPIncorrect = "OTPIncorrect";
    }
}
