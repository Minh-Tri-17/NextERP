using Microsoft.AspNetCore.Http;
using NextERP.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NextERP.ModelBase
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ViewFieldAttribute : Attribute { }

    #region Model Database

    public partial class AppointmentModel : Appointment
    {
        public class AttributeNames
        {
            public const string AppointmentId = "AppointmentId";
            public const string AppointmentCode = "AppointmentCode";
            public const string AppointmentDate = "AppointmentDate";
            public const string AppointmentStatus = "AppointmentStatus";
            public const string TotalCost = "TotalCost";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class AttendanceModel : Attendance
    {
        public class AttributeNames
        {
            public const string AttendanceId = "AttendanceId";
            public const string AttendanceCode = "AttendanceCode";
            public const string WorkDate = "WorkDate";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string WorkingHours = "WorkingHours";
            public const string OvertimeHours = "OvertimeHours";
            public const string EmployeeId = "EmployeeId";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class BranchModel : Branch
    {
        public class AttributeNames
        {
            public const string BranchId = "BranchId";
            public const string BranchCode = "BranchCode";
            public const string BranchName = "BranchName";
            public const string Address = "Address";
            public const string PhoneNumber = "PhoneNumber";
            public const string OperatingStatus = "OperatingStatus";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class CustomerModel : Customer
    {
        public class AttributeNames
        {
            public const string CustomerId = "CustomerId";
            public const string CustomerCode = "CustomerCode";
            public const string FullName = "FullName";
            public const string Gender = "Gender";
            public const string Dob = "Dob";
            public const string PhoneNumber = "PhoneNumber";
            public const string Email = "Email";
            public const string JoinDate = "JoinDate";
            public const string LoyaltyPoints = "LoyaltyPoints";
            public const string TotalSpent = "TotalSpent";
            public const string OperatingStatus = "OperatingStatus";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class DepartmentModel : Department
    {
        public class AttributeNames
        {
            public const string DepartmentId = "DepartmentId";
            public const string DepartmentCode = "DepartmentCode";
            public const string DepartmentName = "DepartmentName";
            public const string NumberOfEmployees = "NumberOfEmployees";
            public const string OperatingStatus = "OperatingStatus";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class EmployeeModel : Employee
    {
        public class AttributeNames
        {
            public const string EmployeeId = "EmployeeId";
            public const string EmployeeCode = "EmployeeCode";
            public const string FullName = "FullName";
            public const string PhoneNumber = "PhoneNumber";
            public const string Gender = "Gender";
            public const string Dob = "Dob";
            public const string Address = "Address";
            public const string NationalId = "NationalId";
            public const string Email = "Email";
            public const string Photo = "Photo";
            public const string EducationLevel = "EducationLevel";
            public const string BankAccountNumber = "BankAccountNumber";
            public const string HealthInsuranceNumber = "HealthInsuranceNumber";
            public const string SocialInsuranceNumber = "SocialInsuranceNumber";
            public const string TaxCode = "TaxCode";
            public const string HireDate = "HireDate";
            public const string OperatingStatus = "OperatingStatus";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class FeedbackModel : Feedback
    {
        public class AttributeNames
        {
            public const string FeedbackId = "FeedbackId";
            public const string FeedbackCode = "FeedbackCode";
            public const string Rating = "Rating";
            public const string Comment = "Comment";
            public const string DateFeedback = "DateFeedback";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class FunctionModel : Function
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class InvoiceModel : Invoice
    {
        public class AttributeNames
        {
            public const string InvoiceId = "InvoiceId";
            public const string InvoiceCode = "InvoiceCode";
            public const string TotalAmount = "TotalAmount";
            public const string InvoiceDate = "InvoiceDate";
            public const string PaymentMethod = "PaymentMethod";
            public const string Discount = "Discount";
            public const string FinalAmount = "FinalAmount";
            public const string PaymentStatus = "PaymentStatus";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class InvoiceDetailModel : InvoiceDetail
    {
        public class AttributeNames
        {
            public const string InvoiceDetailId = "InvoiceDetailId";
            public const string InvoiceDetailCode = "InvoiceDetailCode";
            public const string Quantity = "Quantity";
            public const string UnitPrice = "UnitPrice";
            public const string Discount = "Discount";
            public const string TotalPrice = "TotalPrice";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class LeaveRequestModel : LeaveRequest
    {
        public class AttributeNames
        {
            public const string LeaveRequestId = "LeaveRequestId";
            public const string LeaveRequestCode = "LeaveRequestCode";
            public const string RequestDate = "RequestDate";
            public const string LeaveStartDate = "LeaveStartDate";
            public const string LeaveEndDate = "LeaveEndDate";
            public const string TotalLeaveDays = "TotalLeaveDays";
            public const string LeaveDayType = "LeaveDayType";
            public const string ApprovalStatus = "ApprovalStatus";
            public const string ApprovalDate = "ApprovalDate";
            public const string ApprovedByIds = "ApprovedByIds";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class HistoryMailModel : HistoryMail
    {
        public class AttributeNames
        {
            public const string HistoryMailId = "HistoryMailId";
            public const string HistoryMailCode = "HistoryMailCode";
            public const string HistoryMailName = "HistoryMailName";
            public const string HistoryMailContent = "HistoryMailContent";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class HistoryNotificationModel : HistoryNotification
    {
        public class AttributeNames
        {
            public const string HistoryNotificationId = "HistoryNotificationId";
            public const string HistoryNotificationCode = "HistoryNotificationCode";
            public const string HistoryNotificationName = "HistoryNotificationName";
            public const string HistoryNotificationContent = "HistoryNotificationContent";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class TemplateMailModel : TemplateMail
    {
        public class AttributeNames
        {
            public const string TemplateMailId = "TemplateMailId";
            public const string TemplateMailCode = "TemplateMailCode";
            public const string TemplateMailName = "TemplateMailName";
            public const string TemplateContent = "TemplateContent";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class TemplateNotificationModel : TemplateNotification
    {
        public class AttributeNames
        {
            public const string TemplateNotificationId = "TemplateNotificationId";
            public const string TemplateNotificationCode = "TemplateNotificationCode";
            public const string TemplateNotificationName = "TemplateNotificationName";
            public const string TemplateContent = "TemplateContent";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class PositionModel : Position
    {
        public class AttributeNames
        {
            public const string PositionId = "PositionId";
            public const string PositionCode = "PositionCode";
            public const string PositionName = "PositionName";
            public const string Note = "Note";

        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class ProductModel : Product
    {
        public class AttributeNames
        {
            public const string ProductId = "ProductId";
            public const string ProductCode = "ProductCode";
            public const string ProductName = "ProductName";
            public const string QuantityInStock = "QuantityInStock";
            public const string Price = "Price";
            public const string ExpirationDate = "ExpirationDate";
            public const string Photo = "Photo";
            public const string Manufacturer = "Manufacturer";
            public const string ImageFiles = "ImageFiles";
            public const string CountryOfOrigin = "CountryOfOrigin";
            public const string ReorderLevel = "ReorderLevel";
            public const string Note = "Note";
        }

        [DataType(DataType.Upload)]
        public List<IFormFile>? ImageFiles { get; set; }
        public List<string> Base64Images { get; set; } = new List<string>();
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class ProductImageModel : ProductImage
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class ProductCategoryModel : ProductCategory
    {
        public class AttributeNames
        {
            public const string ProductCategoryId = "ProductCategoryId";
            public const string ProductCategoryCode = "ProductCategoryCode";
            public const string ProductCategoryName = "ProductCategoryName";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class PromotionModel : Promotion
    {
        public class AttributeNames
        {
            public const string PromotionId = "PromotionId";
            public const string PromotionCode = "PromotionCode";
            public const string PromotionName = "PromotionName";
            public const string DiscountPercentage = "DiscountPercentage";
            public const string StartDate = "StartDate";
            public const string EndDate = "EndDate";
            public const string OperatingStatus = "OperatingStatus";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class RoleModel : Role
    {
        public class AttributeNames
        {
            public const string RoleId = "RoleId";
            public const string RoleCode = "RoleCode";
            public const string GroupRole = "GroupRole";
            public const string RoleName = "RoleName";
            public const string Permissions = "Permissions";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SalaryModel : Salary
    {
        public class AttributeNames
        {
            public const string SalaryId = "SalaryId";
            public const string SalaryCode = "SalaryCode";
            public const string SalaryMonth = "SalaryMonth";
            public const string GrossSalary = "GrossSalary";
            public const string Bonus = "Bonus";
            public const string TaxAmount = "TaxAmount";
            public const string InsuranceContribution = "InsuranceContribution";
            public const string AdvanceSalary = "AdvanceSalary";
            public const string Deductions = "Deductions";
            public const string NetSalary = "NetSalary";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class ScheduleModel : Schedule
    {
        public class AttributeNames
        {
            public const string ScheduleId = "ScheduleId";
            public const string ScheduleCode = "ScheduleCode";
            public const string WorkDate = "WorkDate";
            public const string StartTime = "StartTime";
            public const string EndTime = "EndTime";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SpaServiceModel : SpaService
    {
        public class AttributeNames
        {
            public const string SpaServiceId = "SpaServiceId";
            public const string SpaServiceCode = "SpaServiceCode";
            public const string SpaServiceName = "SpaServiceName";
            public const string Price = "Price";
            public const string Duration = "Duration";
            public const string OperatingStatus = "OperatingStatus";
            public const string IsPromotional = "IsPromotional";
            public const string SpaServiceImage = "SpaServiceImage";
            public const string SpaServiceLevel = "SpaServiceLevel";
            public const string SuppliesRequiredIds = "SuppliesRequiredIds";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SpaServiceImageModel : SpaServiceImage
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SpaServiceCategoryModel : SpaServiceCategory
    {
        public class AttributeNames
        {
            public const string SpaServiceCategoryId = "SpaServiceCategoryId";
            public const string SpaServiceCategoryCode = "SpaServiceCategoryCode";
            public const string SpaServiceCategoryName = "SpaServiceCategoryName";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SupplierModel : Supplier
    {
        public class AttributeNames
        {
            public const string SupplierId = "SupplierId";
            public const string SupplierCode = "SupplierCode";
            public const string SupplierName = "SupplierName";
            public const string ContactName = "ContactName";
            public const string PhoneNumber = "PhoneNumber";
            public const string Email = "Email";
            public const string Address = "Address";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SupplierOrderModel : SupplierOrder
    {
        public class AttributeNames
        {
            public const string SupplierOrderId = "SupplierOrderId";
            public const string SupplierOrderCode = "SupplierOrderCode";
            public const string OrderDate = "OrderDate";
            public const string ExpectedDeliveryDate = "ExpectedDeliveryDate";
            public const string ActualDeliveryDate = "ActualDeliveryDate";
            public const string TotalAmount = "TotalAmount";
            public const string OrderStatus = "OrderStatus";
            public const string PaymentStatus = "PaymentStatus";
            public const string Note = "Note";
        }

        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }
        public string Base64Image { get; set; } = string.Empty;
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SupplierOrderDetailModel : SupplierOrderDetail
    {
        public class AttributeNames
        {
            public const string SupplierOrderDetailId = "SupplierOrderDetailId";
            public const string SupplierOrderDetailCode = "SupplierOrderDetailCode";
            public const string Quantity = "Quantity";
            public const string Price = "Price";
            public const string TotalPrice = "TotalPrice";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class TrainingSessionModel : TrainingSession
    {
        public class AttributeNames
        {
            public const string TrainingSessionId = "TrainingSessionId";
            public const string TrainingSessionCode = "TrainingSessionCode";
            public const string TrainingSessionName = "TrainingSessionName";
            public const string Trainer = "Trainer";
            public const string TrainingDate = "TrainingDate";
            public const string Duration = "Duration";
            public const string Participants = "Participants";
            public const string TrainingMaterial = "TrainingMaterial";
            public const string SessionStatus = "SessionStatus";
            public const string Note = "Note";
        }

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class UserModel : User
    {
        public class AttributeNames
        {
            public const string UserId = "UserId";
            public const string UserCode = "UserCode";
            public const string Username = "Username";
            public const string Password = "Password";
            public const string NewPassword = "NewPassword";
            public const string PhoneNumber = "PhoneNumber";
            public const string PhoneNumberRegistered = "PhoneNumberRegistered";
            public const string Otp = "Otp";
            public const string LastLoginDate = "LastLoginDate";
            public const string OperatingStatus = "OperatingStatus";
            public const string GroupRole = "GroupRole";
            public const string Roles = "Roles";
            public const string RoleIds = "RoleIds";
            public const string Remember = "Remember";
            public const string PasswordHash = "PasswordHash";
            public const string Note = "Note";
        }

        public bool Remember { get; set; } = false;
        [Required]
        public string Password { get; set; } = string.Empty;
        [ViewField]
        public string EmployeeName { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    #endregion

    public class MailModel
    {
        [Required]
        public string To { get; set; } = string.Empty;
        [Required]
        public string Subject { get; set; } = string.Empty;
        [Required]
        public string Body { get; set; } = string.Empty;
        public List<string> CC { get; set; } = new List<string>();
        public List<string> BCC { get; set; } = new List<string>();
        public List<IFormFile>? Attachments { get; set; }
    }

    public class FilterModel
    {
        public bool AllowPaging { get; set; } = true;
        public Guid? IdMain { get; set; }
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public List<FilterItemModel> Filters { get; set; } = new List<FilterItemModel>();
    }

    public class FilterItemModel
    {
        public string FilterName { get; set; } = string.Empty;
        public string FilterValue { get; set; } = string.Empty;
        public string FilterType { get; set; } = string.Empty;
        public string FilterOperator { get; set; } = string.Empty;
    }

    public class CalendarModel
    {
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool AllDay { get; set; } = false;
        public string Description { get; set; } = string.Empty;
    }

    public class DataChartNumericModel
    {
        public List<NumericSeriesModel>? Values { get; set; }
        public string[]? Labels { get; set; }
    }

    public class NumericSeriesModel
    {
        public string Name { get; set; } = string.Empty;
        public int[]? Data { get; set; }
    }

    public class DataChartSingleModel
    {
        public int[]? Values { get; set; }
        public string[]? Labels { get; set; }
    }

    public class DataChartXYModel
    {
        public List<XYSeriesModel>? Values { get; set; }
        public string[]? Labels { get; set; }
    }

    public class XYSeriesModel
    {
        public string Name { get; set; } = string.Empty;
        public List<XYPointModel>? Data { get; set; }
    }

    public class XYPointModel
    {
        public string X { get; set; } = string.Empty;
        public int Y { get; set; }
    }

    public class DashboardModel
    {
        public decimal StatisticsProfit { get; set; } = 0;
        public decimal StatisticsRevenue { get; set; } = 0;
        public decimal StatisticsSpending { get; set; } = 0;
        public int StatisticsCustomer { get; set; } = 0;
        public List<string> StatisticsService { get; set; } = new List<string>();
    }

    public class ButtonComponentViewModel
    {
        public string NameButton { get; set; } = string.Empty;

        public string IconHtml { get; set; } = string.Empty;
    }
}
