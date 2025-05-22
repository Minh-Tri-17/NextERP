namespace NextERP.Util
{
    public class Messages
    {
        #region Messages base

        public const string NotFoundUpdate = "Update failed: Not found ID";
        public const string NotFoundGet = "Get failed: Not found ID";
        public const string NotFoundGetList = "Get list failed: List result is null";
        public const string CreateSuccess = "Create succeeded";
        public const string UpdateSuccess = "Update succeeded";
        public const string DeleteSuccess = "Delete succeeded";
        public const string ImportSuccess = "Import succeeded";
        public const string ExportSuccess = "Export succeeded";
        public const string CreateFailed = "Create failed";
        public const string UpdateFailed = "Update failed";
        public const string DeleteFailed = "Delete failed";
        public const string ImportFailed = "Import failed";
        public const string ExportFailed = "Export failed";
        public const string GetListResultSuccess = "Get list succeeded";
        public const string GetResultSuccess = "Get succeeded";

        #endregion

        #region Meassages Auth

        public const string AuthSuccess = "Authenticate succeeded";
        public const string UserNameNotExist = "UserName {0} is not exist.";
        public const string UserNameExist = "UserName {0} is exist.";
        public const string UserNameOrPasswordIncorrect = "UserName {0} or Password {1} is incorrect.";
        public const string TokenKeyNotConfigured = "Token key is not configured properly in the application settings.";
        public const string RoleNotExist = "Role Customer is not exist.";

        #endregion
    }
}
