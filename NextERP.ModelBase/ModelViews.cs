using Microsoft.AspNetCore.Http;
using NextERP.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NextERP.ModelBase
{
    #region Model Database

    public partial class AppointmentModel : Appointment
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class AttendanceModel : Attendance
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class BranchModel : Branch
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class CustomerModel : Customer
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class DepartmentModel : Department
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class EmployeeModel : Employee
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class FeedbackModel : Feedback
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class FunctionModel : Function
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class InvoiceModel : Invoice
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class InvoiceDetailModel : InvoiceDetail
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class LeaveRequestModel : LeaveRequest
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class NotificationModel : Notification
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class PositionModel : Position
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class ProductModel : Product
    {
        [DataType(DataType.Upload)]
        public List<IFormFile>? ImageFiles { get; set; }
        public List<string> Base64Images { get; set; } = new List<string>();

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class ProductImageModel : ProductImage
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class ProductCategoryModel : ProductCategory
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class PromotionModel : Promotion
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class RoleModel : Role
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class SalaryModel : Salary
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class ScheduleModel : Schedule
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class SpaServiceModel : SpaService
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class SpaServiceImageModel : SpaServiceImage
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class SpaServiceCategoryModel : SpaServiceCategory
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class SupplierModel : Supplier
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class SupplierOrderModel : SupplierOrder
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class SupplierOrderDetailModel : SupplierOrderDetail
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class TrainingSessionModel : TrainingSession
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public partial class UserModel : User
    {
        public bool Remember { get; set; } = false;
        public string Password { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;

        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    #endregion

    public class Filter
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? KeyWord { get; set; }
        public bool AllowPaging { get; set; } = true;
        public bool IsDelete { get; set; } = false;
        public string? Ids { get; set; }
        public Guid? IdMain { get; set; }
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }

    public class Calendar
    {
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool AllDay { get; set; } = false;
        public string Description { get; set; } = string.Empty;
    }

    public class DataImport
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class DataChartNumeric
    {
        public List<NumericSeries>? Values { get; set; }
        public string[]? Labels { get; set; }
    }

    public class NumericSeries
    {
        public string Name { get; set; } = string.Empty;
        public int[]? Data { get; set; }
    }

    public class DataChartSingle
    {
        public int[]? Values { get; set; }
        public string[]? Labels { get; set; }
    }

    public class DataChartXY
    {
        public List<XYSeries>? Values { get; set; }
        public string[]? Labels { get; set; }   // Có thể null nếu không dùng
    }

    public class XYSeries
    {
        public string Name { get; set; } = string.Empty;
        public List<XYPoint>? Data { get; set; }
    }

    public class XYPoint
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
}
