using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.ModelBase
{
    public class TableName
    {
        public const string Appointment = "Appointment";
        public const string Attendance = "Attendance";
        public const string Branch = "Branch";
        public const string Customer = "Customer";
        public const string Department = "Department";
        public const string Employee = "Employee";
        public const string Feedback = "Feedback";
        public const string Function = "Function";
        public const string Invoice = "Invoice";
        public const string InvoiceDetail = "InvoiceDetail";
        public const string LeaveRequest = "LeaveRequest";
        public const string Notification = "Notification";
        public const string Position = "Position";
        public const string Product = "Product";
        public const string ProductCategory = "ProductCategory";
        public const string Promotion = "Promotion";
        public const string Role = "Role";
        public const string Salary = "Salary";
        public const string Schedule = "Schedule";
        public const string SpaService = "SpaService";
        public const string SpaServiceCategory = "SpaServiceCategory";
        public const string Supplier = "Supplier";
        public const string SupplierOrder = "SupplierOrder";
        public const string SupplierOrderDetail = "SupplierOrderDetail";
        public const string TrainingSession = "TrainingSession";
        public const string User = "User";
    }

    public class IDName
    {
        public class Appointment
        {
            public const string appointmentDelete = "appointmentDelete";
            public const string appointmentTable = "appointmentTable";
            public const string appointmentImport = "appointmentImport";
        }

        public class Attendance
        {
            public const string attendanceDelete = "attendanceDelete";
            public const string attendanceTable = "attendanceTable";
            public const string attendanceImport = "attendanceImport";
        }

        public class Branch
        {
            public const string branchDelete = "branchDelete";
            public const string branchTable = "branchTable";
            public const string branchImport = "branchImport";
        }

        public class Customer
        {
            public const string customerDelete = "customerDelete";
            public const string customerTable = "customerTable";
            public const string customerImport = "customerImport";
        }

        public class Department
        {
            public const string departmentDelete = "departmentDelete";
            public const string departmentTable = "departmentTable";
            public const string departmentImport = "departmentImport";
        }

        public class Employee
        {
            public const string employeeDelete = "employeeDelete";
            public const string employeeTable = "employeeTable";
            public const string employeeImport = "employeeImport";
        }

        public class Feedback
        {
            public const string feedbackDelete = "feedbackDelete";
            public const string feedbackTable = "feedbackTable";
            public const string feedbackImport = "feedbackImport";
        }

        public class Function
        {
            public const string functionDelete = "functionDelete";
            public const string functionTable = "functionTable";
            public const string functionImport = "functionImport";
        }

        public class Invoice
        {
            public const string invoiceDelete = "invoiceDelete";
            public const string invoiceTable = "invoiceTable";
            public const string invoiceImport = "invoiceImport";
        }

        public class InvoiceDetail
        {
            public const string invoiceDetailDelete = "invoiceDetailDelete";
            public const string invoiceDetailTable = "invoiceDetailTable";
            public const string invoiceDetailImport = "invoiceDetailImport";
        }

        public class LeaveRequest
        {
            public const string leaveRequestDelete = "leaveRequestDelete";
            public const string leaveRequestTable = "leaveRequestTable";
            public const string leaveRequestImport = "leaveRequestImport";
        }

        public class Notification
        {
            public const string notificationDelete = "notificationDelete";
            public const string notificationTable = "notificationTable";
            public const string notificationImport = "notificationImport";
        }

        public class Position
        {
            public const string positionDelete = "positionDelete";
            public const string positionTable = "positionTable";
            public const string positionImport = "positionImport";
        }

        public class Product
        {
            public const string productDelete = "productDelete";
            public const string productTable = "productTable";
            public const string productImport = "productImport";
        }

        public class ProductCategory
        {
            public const string productCategoryDelete = "productCategoryDelete";
            public const string productCategoryTable = "productCategoryTable";
            public const string productCategoryImport = "productCategoryImport";
        }

        public class Promotion
        {
            public const string promotionDelete = "promotionDelete";
            public const string promotionTable = "promotionTable";
            public const string promotionImport = "promotionImport";
        }

        public class Role
        {
            public const string roleDelete = "roleDelete";
            public const string roleTable = "roleTable";
            public const string roleImport = "roleImport";
        }

        public class Salary
        {
            public const string salaryDelete = "salaryDelete";
            public const string salaryTable = "salaryTable";
            public const string salaryImport = "salaryImport";
        }

        public class Schedule
        {
            public const string scheduleDelete = "scheduleDelete";
            public const string scheduleTable = "scheduleTable";
            public const string scheduleImport = "scheduleImport";
        }

        public class SpaService
        {
            public const string serviceDelete = "serviceDelete";
            public const string serviceTable = "serviceTable";
            public const string serviceImport = "serviceImport";
        }

        public class SpaServiceCategory
        {
            public const string serviceCategoryDelete = "serviceCategoryDelete";
            public const string serviceCategoryTable = "serviceCategoryTable";
            public const string serviceCategoryImport = "serviceCategoryImport";
        }

        public class Supplier
        {
            public const string supplierDelete = "supplierDelete";
            public const string supplierTable = "supplierTable";
            public const string supplierImport = "supplierImport";
        }

        public class SupplierOrder
        {
            public const string supplierOrderDelete = "supplierOrderDelete";
            public const string supplierOrderTable = "supplierOrderTable";
            public const string supplierOrderImport = "supplierOrderImport";
        }

        public class SupplierOrderDetail
        {
            public const string supplierOrderDetailDelete = "supplierOrderDetailDelete";
            public const string supplierOrderDetailTable = "supplierOrderDetailTable";
            public const string supplierOrderDetailImport = "supplierOrderDetailImport";
        }

        public class TrainingSession
        {
            public const string trainingSessionDelete = "trainingSessionDelete";
            public const string trainingSessionTable = "trainingSessionTable";
            public const string trainingSessionImport = "trainingSessionImport";
        }

        public class User
        {
            public const string userDelete = "userDelete";
            public const string userTable = "userTable";
            public const string userImport = "userImport";
        }
    }

    public class ScreenName
    {
        public const string DashboardIndex = "DashboardIndex";
        public const string AccountIndex = "AccountIndex";

        public class Appointment
        {
            public const string AppointmentIndex = "AppointmentIndex";
            public const string AppointmentForm = "AppointmentForm";
            public const string AppointmentList = "AppointmentList";
        }

        public class Attendance
        {
            public const string AttendanceIndex = "AttendanceIndex";
            public const string AttendanceForm = "AttendanceForm";
            public const string AttendanceList = "AttendanceList";
        }

        public class Branch
        {
            public const string BranchIndex = "BranchIndex";
            public const string BranchForm = "BranchForm";
            public const string BranchList = "BranchList";
        }

        public class Customer
        {
            public const string CustomerIndex = "CustomerIndex";
            public const string CustomerForm = "CustomerForm";
            public const string CustomerList = "CustomerList";
        }

        public class Department
        {
            public const string DepartmentIndex = "DepartmentIndex";
            public const string DepartmentForm = "DepartmentForm";
            public const string DepartmentList = "DepartmentList";
        }

        public class Employee
        {
            public const string EmployeeIndex = "EmployeeIndex";
            public const string EmployeeForm = "EmployeeForm";
            public const string EmployeeList = "EmployeeList";
        }

        public class Feedback
        {
            public const string FeedbackIndex = "FeedbackIndex";
            public const string FeedbackForm = "FeedbackForm";
            public const string FeedbackList = "FeedbackList";
        }

        public class Invoice
        {
            public const string InvoiceIndex = "InvoiceIndex";
            public const string InvoiceForm = "InvoiceForm";
            public const string InvoiceList = "InvoiceList";
        }

        public class InvoiceDetail
        {
            public const string InvoiceDetailIndex = "InvoiceDetailIndex";
            public const string InvoiceDetailList = "InvoiceDetailList";
        }

        public class LeaveRequest
        {
            public const string LeaveRequestIndex = "LeaveRequestIndex";
            public const string LeaveRequestForm = "LeaveRequestForm";
            public const string LeaveRequestList = "LeaveRequestList";
        }

        public class Notification
        {
            public const string NotificationIndex = "NotificationIndex";
            public const string NotificationForm = "NotificationForm";
            public const string NotificationList = "NotificationList";
        }

        public class Position
        {
            public const string PositionIndex = "PositionIndex";
            public const string PositionForm = "PositionForm";
            public const string PositionList = "PositionList";
        }

        public class Product
        {
            public const string ProductIndex = "ProductIndex";
            public const string ProductForm = "ProductForm";
            public const string ProductList = "ProductList";
        }

        public class ProductCategory
        {
            public const string ProductCategoryIndex = "ProductCategoryIndex";
            public const string ProductCategoryForm = "ProductCategoryForm";
            public const string ProductCategoryList = "ProductCategoryList";
        }

        public class Promotion
        {
            public const string PromotionIndex = "PromotionIndex";
            public const string PromotionForm = "PromotionForm";
            public const string PromotionList = "PromotionList";
        }

        public class Role
        {
            public const string RoleIndex = "RoleIndex";
            public const string RoleForm = "RoleForm";
            public const string RoleList = "RoleList";
        }

        public class Salary
        {
            public const string SalaryIndex = "SalaryIndex";
            public const string SalaryForm = "SalaryForm";
            public const string SalaryList = "SalaryList";
        }

        public class Schedule
        {
            public const string ScheduleIndex = "ScheduleIndex";
            public const string ScheduleForm = "ScheduleForm";
            public const string ScheduleList = "ScheduleList";
        }

        public class SpaService
        {
            public const string SpaServiceIndex = "SpaServiceIndex";
            public const string SpaServicForm = "SpaServiceForm";
            public const string SpaServiceList = "SpaServiceList";
        }

        public class SpaServiceCategory
        {
            public const string SpaServiceCategoryIndex = "SpaServiceCategoryIndex";
            public const string SpaServiceCategoryForm = "SpaServiceCategoryForm";
            public const string SpaServiceCategoryList = "SpaServiceCategoryList";
        }

        public class Supplier
        {
            public const string SupplierIndex = "SupplierIndex";
            public const string SupplierForm = "SupplierForm";
            public const string SupplierList = "SupplierList";
        }

        public class SupplierOrder
        {
            public const string SupplierOrderIndex = "SupplierOrderIndex";
            public const string SupplierOrderForm = "SupplierOrderForm";
            public const string SupplierOrderList = "SupplierOrderList";
        }

        public class SupplierOrderDetail
        {
            public const string SupplierOrderDetailIndex = "SupplierOrderDetailIndex";
            public const string SupplierOrderDetailForm = "SupplierOrderDetailForm";
            public const string SupplierOrderDetailList = "SupplierOrderDetailList";
        }

        public class TrainingSession
        {
            public const string TrainingSessionIndex = "TrainingSessionIndex";
            public const string TrainingSessionForm = "TrainingSessionForm";
            public const string TrainingSessionList = "TrainingSessionList";
        }

        public class User
        {
            public const string UserIndex = "UserIndex";
            public const string UserForm = "UserForm";
            public const string UserList = "UserList";
        }
    }

    public class AttributeNames
    {
        public class Appointment
        {
            public const string AppointmentId = "AppointmentId";
            public const string AppointmentCode = "AppointmentCode";
            public const string AppointmentDate = "AppointmentDate";
            public const string AppointmentStatus = "AppointmentStatus";
            public const string TotalCost = "TotalCost";
            public const string Note = "Note";
        }

        public class Attendance
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

        public class Branch
        {
            public const string BranchId = "BranchId";
            public const string BranchCode = "BranchCode";
            public const string BranchName = "BranchName";
            public const string Address = "Address";
            public const string PhoneNumber = "PhoneNumber";
            public const string OperatingStatus = "OperatingStatus";
            public const string Note = "Note";
        }

        public class Customer
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

        public class Department
        {
            public const string DepartmentId = "DepartmentId";
            public const string DepartmentCode = "DepartmentCode";
            public const string DepartmentName = "DepartmentName";
            public const string NumberOfEmployees = "NumberOfEmployees";
            public const string OperatingStatus = "OperatingStatus";
            public const string Note = "Note";
        }

        public class Employee
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

        public class Feedback
        {
            public const string FeedbackId = "FeedbackId";
            public const string FeedbackCode = "FeedbackCode";
            public const string Rating = "Rating";
            public const string Comment = "Comment";
            public const string DateFeedback = "DateFeedback";
        }

        public class Function
        {

        }

        public class Invoice
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

        public class InvoiceDetail
        {
            public const string InvoiceDetailId = "InvoiceDetailId";
            public const string InvoiceDetailCode = "InvoiceDetailCode";
            public const string Quantity = "Quantity";
            public const string UnitPrice = "UnitPrice";
            public const string Discount = "Discount";
            public const string TotalPrice = "TotalPrice";
            public const string Note = "Note";
        }

        public class LeaveRequest
        {
            public const string LeaveRequestId = "LeaveRequestId";
            public const string LeaveRequestCode = "LeaveRequestCode";
            public const string RequestDate = "RequestDate";
            public const string LeaveStartDate = "LeaveStartDate";
            public const string LeaveEndDate = "LeaveEndDate";
            public const string TotalLeaveDays = "TotalLeaveDays";
            public const string LeaveDayType = "LeaveDayType";
            public const string RequestStatus = "RequestStatus";
            public const string ApprovalDate = "ApprovalDate";
            public const string ApprovedByIds = "ApprovedByIds";
            public const string Note = "Note";
        }

        public class Notification
        {
            public const string NotificationId = "NotificationId";
            public const string NotificationCode = "NotificationCode";
            public const string NotificationName = "NotificationName";
            public const string NotificationContent = "NotificationContent";
            public const string Note = "Note";
        }

        public class Position
        {
            public const string PositionId = "PositionId";
            public const string PositionCode = "PositionCode";
            public const string PositionName = "PositionName";
            public const string Note = "Note";

        }

        public class Product
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

        public class ProductCategory
        {
            public const string ProductCategoryId = "ProductCategoryId";
            public const string ProductCategoryCode = "ProductCategoryCode";
            public const string CategoryName = "CategoryName";
            public const string Note = "Note";
        }

        public class Promotion
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

        public class Role
        {
            public const string RoleId = "RoleId";
            public const string RoleCode = "RoleCode";
            public const string GroupRole = "GroupRole";
            public const string RoleName = "RoleName";
            public const string Permissions = "Permissions";
            public const string Note = "Note";
        }

        public class Salary
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

        public class Schedule
        {
            public const string ScheduleId = "ScheduleId";
            public const string ScheduleCode = "ScheduleCode";
            public const string WorkDate = "WorkDate";
            public const string StartTime = "StartTime";
            public const string EndTime = "EndTime";
            public const string Note = "Note";
        }

        public class SpaService
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

        public class SpaServiceCategory
        {
            public const string SpaServiceCategoryId = "SpaServiceCategoryId";
            public const string SpaServiceCategoryCode = "SpaServiceCategoryCode";
            public const string SpaServiceCategoryName = "SpaServiceCategoryName";
            public const string Note = "Note";
        }

        public class Supplier
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

        public class SupplierOrder
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

        public class SupplierOrderDetail
        {
            public const string SupplierOrderDetailId = "SupplierOrderDetailId";
            public const string SupplierOrderDetailCode = "SupplierOrderDetailCode";
            public const string Quantity = "Quantity";
            public const string Price = "Price";
            public const string TotalPrice = "TotalPrice";
            public const string Note = "Note";
        }

        public class TrainingSession
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

        public class User
        {
            public const string UserId = "UserId";
            public const string UserCode = "UserCode";
            public const string Username = "Username";
            public const string Password = "Password";
            public const string PhoneNumber = "PhoneNumber";
            public const string LastLoginDate = "LastLoginDate";
            public const string OperatingStatus = "OperatingStatus";
            public const string GroupRole = "GroupRole";
            public const string Roles = "Roles";
            public const string RoleIds = "RoleIds";
            public const string Remember = "Remember";
            public const string PasswordHash = "PasswordHash";
            public const string Note = "Note";
        }
    }
}
