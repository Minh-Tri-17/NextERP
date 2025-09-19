namespace NextERP.Util
{
    public class Constants
    {
        public const string Context = "Context";
        public const string Token = "Token";
        public const string APIAddress = "APIAddress";
        public const string Id = "Id";
        public const string Ids = "Ids";
        public const string IdMain = "IdMain";
        public const string Code = "Code";
        public const string AllowPaging = "AllowPaging";
        public const string ImageFiles = "ImageFiles";
        public const string PageIndex = "PageIndex";
        public const string PageSize = "PageSize";
        public const string IsDelete = "IsDelete";
        public const string UserCreate = "UserCreate";
        public const string UserUpdate = "UserUpdate";
        public const string DateCreate = "DateCreate";
        public const string DateUpdate = "DateUpdate";
        public const string ExcelFiles = "ExcelFiles";
        public const string Files = "Files";
        public const string Filter = "Filter";
        public const string SearchBox = "SearchBox";
        public const string Pagination = "Pagination";
        public const string DateFrom = "DateFrom";
        public const string DateTo = "DateTo";
        public const string KeyWord = "KeyWord";
        public const string Base64Images = "Base64Images";
        public const string CreateOrEdit = "CreateOrEdit";
        public const string GetList = "GetList";
        public const string Delete = "Delete";
        public const string Import = "Import";
        public const string Export = "Export";
        public const string SendMail = "SendMail";
        public const string DeletePermanently = "DeletePermanently";
        public const string Dashboard = "Dashboard";
        public const string GetChartColumn = "GetChartColumn";
        public const string GetChartRadar = "GetChartRadar";
        public const string GetChartDonut = "GetChartDonut";
        public const string GetChartLine = "GetChartLine";
        public const string GetChartFunnel = "GetChartFunnel";
        public const string GetChartSlope = "GetChartSlope";

        #region Format

        public const string FileName = "{0}_{1}.xlsx";
        public const string Currency = "$";
        public const string DateTimeString = "yyyyMMddHHmmssfff";
        public const string DateFirstFormatDash = "dd-MM-yyyy";
        public const string DateLastFormatDash = "yyyy-MM-dd";
        public const string DateFirstFormatSlash = "dd/MM/yyyy";
        public const string DateLastFormatSlash = "yyyy/MM/dd";
        public const string DateTimeFormatDash = "dd-MM-yyyy HH:mm:ss";
        public const string DateTimeFormatSlash = "dd/MM/yyyy HH:mm:ss";
        public const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        #endregion

        #region Url API

        #region Account

        public const string UrlAuthentication = "/api/Account/Authentication";
        public const string UrlRegister = "/api/Account/Register";

        #endregion

        #region Appointment

        public const string UrlCreateOrEditAppointment = "/api/Appointment/CreateOrEditAppointment";
        public const string UrlDeleteAppointment = "/api/Appointment/DeleteAppointment";
        public const string UrlDeletePermanentlyAppointment = "/api/Appointment/DeletePermanentlyAppointment";
        public const string UrlGetAppointment = "/api/Appointment/GetAppointment";
        public const string UrlGetAppointments = "/api/Appointment/GetAppointments";
        public const string UrlImportAppointment = "/api/Appointment/ImportAppointment";
        public const string UrlExportAppointment = "/api/Appointment/ExportAppointment";

        #endregion

        #region Attendance

        public const string UrlCreateOrEditAttendance = "/api/Attendance/CreateOrEditAttendance";
        public const string UrlDeleteAttendance = "/api/Attendance/DeleteAttendance";
        public const string UrlDeletePermanentlyAttendance = "/api/Attendance/DeletePermanentlyAttendance";
        public const string UrlGetAttendance = "/api/Attendance/GetAttendance";
        public const string UrlGetAttendances = "/api/Attendance/GetAttendances";
        public const string UrlImportAttendance = "/api/Attendance/ImportAttendance";
        public const string UrlExportAttendance = "/api/Attendance/ExportAttendance";

        #endregion

        #region Branch

        public const string UrlCreateOrEditBranch = "/api/Branch/CreateOrEditBranch";
        public const string UrlDeleteBranch = "/api/Branch/DeleteBranch";
        public const string UrlDeletePermanentlyBranch = "/api/Branch/DeletePermanentlyBranch";
        public const string UrlGetBranch = "/api/Branch/GetBranch";
        public const string UrlGetBranches = "/api/Branch/GetBranches";
        public const string UrlImportBranch = "/api/Branch/ImportBranch";
        public const string UrlExportBranch = "/api/Branch/ExportBranch";

        #endregion

        #region Customer

        public const string UrlCreateOrEditCustomer = "/api/Customer/CreateOrEditCustomer";
        public const string UrlDeleteCustomer = "/api/Customer/DeleteCustomer";
        public const string UrlDeletePermanentlyCustomer = "/api/Customer/DeletePermanentlyCustomer";
        public const string UrlGetCustomer = "/api/Customer/GetCustomer";
        public const string UrlGetCustomers = "/api/Customer/GetCustomers";
        public const string UrlImportCustomer = "/api/Customer/ImportCustomer";
        public const string UrlExportCustomer = "/api/Customer/ExportCustomer";

        #endregion

        #region Dashboard

        public const string UrlGetChartColumn = "/api/Dashboard/GetChartColumn";
        public const string UrlGetChartDonut = "/api/Dashboard/GetChartDonut";
        public const string UrlGetChartRadar = "/api/Dashboard/GetChartRadar";
        public const string UrlGetChartLine = "/api/Dashboard/GetChartLine";
        public const string UrlGetChartSlope = "/api/Dashboard/GetChartSlope";
        public const string UrlGetChartFunnel = "/api/Dashboard/GetChartFunnel";
        public const string UrlGetStatisticsProfit = "/api/Dashboard/GetStatisticsProfit";
        public const string UrlGetStatisticsRevenue = "/api/Dashboard/GetStatisticsRevenue";
        public const string UrlGetStatisticsSpending = "/api/Dashboard/GetStatisticsSpending";
        public const string UrlGetStatisticsCustomer = "/api/Dashboard/GetStatisticsCustomer";
        public const string UrlGetStatisticsService = "/api/Dashboard/GetStatisticsService";

        #endregion

        #region Department

        public const string UrlCreateOrEditDepartment = "/api/Department/CreateOrEditDepartment";
        public const string UrlDeleteDepartment = "/api/Department/DeleteDepartment";
        public const string UrlDeletePermanentlyDepartment = "/api/Department/DeletePermanentlyDepartment";
        public const string UrlGetDepartment = "/api/Department/GetDepartment";
        public const string UrlGetDepartments = "/api/Department/GetDepartments";
        public const string UrlImportDepartment = "/api/Department/ImportDepartment";
        public const string UrlExportDepartment = "/api/Department/ExportDepartment";

        #endregion

        #region Employee

        public const string UrlCreateOrEditEmployee = "/api/Employee/CreateOrEditEmployee";
        public const string UrlDeleteEmployee = "/api/Employee/DeleteEmployee";
        public const string UrlDeletePermanentlyEmployee = "/api/Employee/DeletePermanentlyEmployee";
        public const string UrlGetEmployee = "/api/Employee/GetEmployee";
        public const string UrlGetEmployees = "/api/Employee/GetEmployees";
        public const string UrlImportEmployee = "/api/Employee/ImportEmployee";
        public const string UrlExportEmployee = "/api/Employee/ExportEmployee";

        #endregion

        #region Feedback

        public const string UrlCreateOrEditFeedback = "/api/Feedback/CreateOrEditFeedback";
        public const string UrlDeleteFeedback = "/api/Feedback/DeleteFeedback";
        public const string UrlDeletePermanentlyFeedback = "/api/Feedback/DeletePermanentlyFeedback";
        public const string UrlGetFeedback = "/api/Feedback/GetFeedback";
        public const string UrlGetFeedbacks = "/api/Feedback/GetFeedbacks";
        public const string UrlImportFeedback = "/api/Feedback/ImportFeedback";
        public const string UrlExportFeedback = "/api/Feedback/ExportFeedback";

        #endregion

        #region Function

        public const string UrlCreateOrEditFunction = "/api/Function/CreateOrEditFunction";
        public const string UrlDeleteFunction = "/api/Function/DeleteFunction";
        public const string UrlDeletePermanentlyFunction = "/api/Function/DeletePermanentlyFunction";
        public const string UrlGetFunction = "/api/Function/GetFunction";
        public const string UrlGetFunctions = "/api/Function/GetFunctions";

        #endregion

        #region Invoice

        public const string UrlCreateOrEditInvoice = "/api/Invoice/CreateOrEditInvoice";
        public const string UrlDeleteInvoice = "/api/Invoice/DeleteInvoice";
        public const string UrlDeletePermanentlyInvoice = "/api/Invoice/DeletePermanentlyInvoice";
        public const string UrlGetInvoice = "/api/Invoice/GetInvoice";
        public const string UrlGetInvoices = "/api/Invoice/GetInvoices";

        #endregion

        #region InvoiceDetail

        public const string UrlCreateOrEditInvoiceDetail = "/api/InvoiceDetail/CreateOrEditInvoiceDetail";
        public const string UrlDeleteInvoiceDetail = "/api/InvoiceDetail/DeleteInvoiceDetail";
        public const string UrlGetInvoiceDetail = "/api/InvoiceDetail/GetInvoiceDetail";
        public const string UrlGetInvoiceDetails = "/api/InvoiceDetail/GetInvoiceDetails";

        #endregion

        #region LeaveRequest

        public const string UrlCreateOrEditLeaveRequest = "/api/LeaveRequest/CreateOrEditLeaveRequest";
        public const string UrlDeleteLeaveRequest = "/api/LeaveRequest/DeleteLeaveRequest";
        public const string UrlDeletePermanentlyLeaveRequest = "/api/LeaveRequest/DeletePermanentlyLeaveRequest";
        public const string UrlGetLeaveRequest = "/api/LeaveRequest/GetLeaveRequest";
        public const string UrlGetLeaveRequests = "/api/LeaveRequest/GetLeaveRequests";
        public const string UrlImportLeaveRequest = "/api/LeaveRequest/ImportLeaveRequest";
        public const string UrlExportLeaveRequest = "/api/LeaveRequest/ExportLeaveRequest";

        #endregion

        #region HistoryNotification

        public const string UrlCreateOrEditHistoryNotification = "/api/HistoryNotification/CreateOrEditHistoryNotification";
        public const string UrlDeleteHistoryNotification = "/api/HistoryNotification/DeleteHistoryNotification";
        public const string UrlDeletePermanentlyHistoryNotification = "/api/HistoryNotification/DeletePermanentlyHistoryNotification";
        public const string UrlGetHistoryNotification = "/api/HistoryNotification/GetHistoryNotification";
        public const string UrlGetHistoryNotifications = "/api/HistoryNotification/GetHistoryNotifications";
        public const string UrlImportHistoryNotification = "/api/HistoryNotification/ImportHistoryNotification";
        public const string UrlExportHistoryNotification = "/api/HistoryNotification/ExportHistoryNotification";

        #endregion

        #region TemplateNotification

        public const string UrlCreateOrEditTemplateNotification = "/api/TemplateNotification/CreateOrEditTemplateNotification";
        public const string UrlDeleteTemplateNotification = "/api/TemplateNotification/DeleteTemplateNotification";
        public const string UrlDeletePermanentlyTemplateNotification = "/api/TemplateNotification/DeletePermanentlyTemplateNotification";
        public const string UrlGetTemplateNotification = "/api/TemplateNotification/GetTemplateNotification";
        public const string UrlGetTemplateNotifications = "/api/TemplateNotification/GetTemplateNotifications";
        public const string UrlImportTemplateNotification = "/api/TemplateNotification/ImportTemplateNotification";
        public const string UrlExportTemplateNotification = "/api/TemplateNotification/ExportTemplateNotification";

        #endregion

        #region HistoryMail

        public const string UrlCreateOrEditHistoryMail = "/api/HistoryMail/CreateOrEditHistoryMail";
        public const string UrlDeleteHistoryMail = "/api/HistoryMail/DeleteHistoryMail";
        public const string UrlDeletePermanentlyHistoryMail = "/api/HistoryMail/DeletePermanentlyHistoryMail";
        public const string UrlGetHistoryMail = "/api/HistoryMail/GetHistoryMail";
        public const string UrlGetHistoryMails = "/api/HistoryMail/GetHistoryMails";
        public const string UrlImportHistoryMail = "/api/HistoryMail/ImportHistoryMail";
        public const string UrlExportHistoryMail = "/api/HistoryMail/ExportHistoryMail";

        #endregion

        #region TemplateMail

        public const string UrlCreateOrEditTemplateMail = "/api/TemplateMail/CreateOrEditTemplateMail";
        public const string UrlDeleteTemplateMail = "/api/TemplateMail/DeleteTemplateMail";
        public const string UrlDeletePermanentlyTemplateMail = "/api/TemplateMail/DeletePermanentlyTemplateMail";
        public const string UrlGetTemplateMail = "/api/TemplateMail/GetTemplateMail";
        public const string UrlGetTemplateMails = "/api/TemplateMail/GetTemplateMails";
        public const string UrlImportTemplateMail = "/api/TemplateMail/ImportTemplateMail";
        public const string UrlExportTemplateMail = "/api/TemplateMail/ExportTemplateMail";
        public const string UrlSendMailTemplateMail = "/api/TemplateMail/SendMailTemplateMail";

        #endregion

        #region Position

        public const string UrlCreateOrEditPosition = "/api/Position/CreateOrEditPosition";
        public const string UrlDeletePosition = "/api/Position/DeletePosition";
        public const string UrlDeletePermanentlyPosition = "/api/Position/DeletePermanentlyPosition";
        public const string UrlGetPosition = "/api/Position/GetPosition";
        public const string UrlGetPositions = "/api/Position/GetPositions";
        public const string UrlImportPosition = "/api/Position/ImportPosition";
        public const string UrlExportPosition = "/api/Position/ExportPosition";

        #endregion

        #region Product

        public const string UrlCreateOrEditProduct = "/api/Product/CreateOrEditProduct";
        public const string UrlDeleteProduct = "/api/Product/DeleteProduct";
        public const string UrlDeletePermanentlyProduct = "/api/Product/DeletePermanentlyProduct";
        public const string UrlGetProduct = "/api/Product/GetProduct";
        public const string UrlGetProducts = "/api/Product/GetProducts";
        public const string UrlImportProduct = "/api/Product/ImportProduct";
        public const string UrlExportProduct = "/api/Product/ExportProduct";
        public const string UrlGetImageProduct = "/api/Product/GetImage/{0}/Image/{1}";

        #endregion

        #region ProductCategory

        public const string UrlCreateOrEditProductCategory = "/api/ProductCategory/CreateOrEditProductCategory";
        public const string UrlDeleteProductCategory = "/api/ProductCategory/DeleteProductCategory";
        public const string UrlDeletePermanentlyProductCategory = "/api/ProductCategory/DeletePermanentlyProductCategory";
        public const string UrlGetProductCategory = "/api/ProductCategory/GetProductCategory";
        public const string UrlGetProductCategories = "/api/ProductCategory/GetProductCategories";
        public const string UrlImportProductCategory = "/api/ProductCategory/ImportProductCategory";
        public const string UrlExportProductCategory = "/api/ProductCategory/ExportProductCategory";

        #endregion

        #region Promotion

        public const string UrlCreateOrEditPromotion = "/api/Promotion/CreateOrEditPromotion";
        public const string UrlDeletePromotion = "/api/Promotion/DeletePromotion";
        public const string UrlDeletePermanentlyPromotion = "/api/Promotion/DeletePermanentlyPromotion";
        public const string UrlGetPromotion = "/api/Promotion/GetPromotion";
        public const string UrlGetPromotions = "/api/Promotion/GetPromotions";
        public const string UrlImportPromotion = "/api/Promotion/ImportPromotion";
        public const string UrlExportPromotion = "/api/Promotion/ExportPromotion";

        #endregion

        #region Role

        public const string UrlCreateOrEditRole = "/api/Role/CreateOrEditRole";
        public const string UrlDeleteRole = "/api/Role/DeleteRole";
        public const string UrlDeletePermanentlyRole = "/api/Role/DeletePermanentlyRole";
        public const string UrlGetRole = "/api/Role/GetRole";
        public const string UrlGetRoles = "/api/Role/GetRoles";
        public const string UrlImportRole = "/api/Role/ImportRole";
        public const string UrlExportRole = "/api/Role/ExportRole";

        #endregion

        #region Salary

        public const string UrlCreateOrEditSalary = "/api/Salary/CreateOrEditSalary";
        public const string UrlDeleteSalary = "/api/Salary/DeleteSalary";
        public const string UrlDeletePermanentlySalary = "/api/Salary/DeletePermanentlySalary";
        public const string UrlGetSalary = "/api/Salary/GetSalary";
        public const string UrlGetSalaries = "/api/Salary/GetSalaries";
        public const string UrlImportSalary = "/api/Salary/ImportSalary";
        public const string UrlExportSalary = "/api/Salary/ExportSalary";

        #endregion

        #region Schedule

        public const string UrlCreateOrEditSchedule = "/api/Schedule/CreateOrEditSchedule";
        public const string UrlDeleteSchedule = "/api/Schedule/DeleteSchedule";
        public const string UrlDeletePermanentlySchedule = "/api/Schedule/DeletePermanentlySchedule";
        public const string UrlGetSchedule = "/api/Schedule/GetSchedule";
        public const string UrlGetSchedules = "/api/Schedule/GetSchedules";
        public const string UrlImportSchedule = "/api/Schedule/ImportSchedule";
        public const string UrlExportSchedule = "/api/Schedule/ExportSchedule";

        #endregion

        #region SpaService

        public const string UrlCreateOrEditSpaService = "/api/SpaService/CreateOrEditSpaService";
        public const string UrlDeleteSpaService = "/api/SpaService/DeleteSpaService";
        public const string UrlDeletePermanentlySpaService = "/api/SpaService/DeletePermanentlySpaService";
        public const string UrlGetSpaService = "/api/SpaService/GetSpaService";
        public const string UrlGetSpaServices = "/api/SpaService/GetSpaServices";
        public const string UrlImportSpaService = "/api/SpaService/ImportSpaService";
        public const string UrlExportSpaService = "/api/SpaService/ExportSpaService";

        #endregion

        #region SpaServiceCategory

        public const string UrlCreateOrEditSpaServiceCategory = "/api/SpaServiceCategory/CreateOrEditSpaServiceCategory";
        public const string UrlDeleteSpaServiceCategory = "/api/SpaServiceCategory/DeleteSpaServiceCategory";
        public const string UrlDeletePermanentlySpaServiceCategory = "/api/SpaServiceCategory/DeletePermanentlySpaServiceCategory";
        public const string UrlGetSpaServiceCategory = "/api/SpaServiceCategory/GetSpaServiceCategory";
        public const string UrlGetSpaServiceCategories = "/api/SpaServiceCategory/GetSpaServiceCategories";
        public const string UrlImportSpaServiceCategory = "/api/SpaServiceCategory/ImportSpaServiceCategory";
        public const string UrlExportSpaServiceCategory = "/api/SpaServiceCategory/ExportSpaServiceCategory";

        #endregion

        #region Supplier

        public const string UrlCreateOrEditSupplier = "/api/Supplier/CreateOrEditSupplier";
        public const string UrlDeleteSupplier = "/api/Supplier/DeleteSupplier";
        public const string UrlDeletePermanentlySupplier = "/api/Supplier/DeletePermanentlySupplier";
        public const string UrlGetSupplier = "/api/Supplier/GetSupplier";
        public const string UrlGetSuppliers = "/api/Supplier/GetSuppliers";
        public const string UrlImportSupplier = "/api/Supplier/ImportSupplier";
        public const string UrlExportSupplier = "/api/Supplier/ExportSupplier";

        #endregion

        #region SupplierOrder

        public const string UrlDeleteSupplierOrder = "/api/SupplierOrder/DeleteSupplierOrder";
        public const string UrlDeletePermanentlySupplierOrder = "/api/SupplierOrder/DeletePermanentlySupplierOrder";
        public const string UrlGetSupplierOrder = "/api/SupplierOrder/GetSupplierOrder";
        public const string UrlGetSupplierOrders = "/api/SupplierOrder/GetSupplierOrders";
        public const string UrlImportSupplierOrder = "/api/SupplierOrder/ImportSupplierOrder";
        public const string UrlExportSupplierOrder = "/api/SupplierOrder/ExportSupplierOrder";

        #endregion

        #region SupplierOrderDetail

        public const string UrlCreateOrEditSupplierOrderDetail = "/api/SupplierOrderDetail/CreateOrEditSupplierOrderDetail";
        public const string UrlDeleteSupplierOrderDetail = "/api/SupplierOrderDetail/DeleteSupplierOrderDetail";
        public const string UrlGetSupplierOrderDetail = "/api/SupplierOrderDetail/GetSupplierOrderDetail";
        public const string UrlGetSupplierOrderDetails = "/api/SupplierOrderDetail/GetSupplierOrderDetails";

        #endregion

        #region TrainingSession

        public const string UrlCreateOrEditTrainingSession = "/api/TrainingSession/CreateOrEditTrainingSession";
        public const string UrlDeleteTrainingSession = "/api/TrainingSession/DeleteTrainingSession";
        public const string UrlDeletePermanentlyTrainingSession = "/api/TrainingSession/DeletePermanentlyTrainingSession";
        public const string UrlGetTrainingSession = "/api/TrainingSession/GetTrainingSession";
        public const string UrlGetTrainingSessions = "/api/TrainingSession/GetTrainingSessions";
        public const string UrlImportTrainingSession = "/api/TrainingSession/ImportTrainingSession";
        public const string UrlExportTrainingSession = "/api/TrainingSession/ExportTrainingSession";

        #endregion

        #region User

        public const string UrlCreateOrEditUser = "/api/User/CreateOrEditUser";
        public const string UrlDeleteUser = "/api/User/DeleteUser";
        public const string UrlDeletePermanentlyUser = "/api/User/DeletePermanentlyUser";
        public const string UrlGetUser = "/api/User/GetUser";
        public const string UrlGetUsers = "/api/User/GetUsers";
        public const string UrlImportUser = "/api/User/ImportUser";
        public const string UrlExportUser = "/api/User/ExportUser";

        #endregion

        #endregion
    }
}
