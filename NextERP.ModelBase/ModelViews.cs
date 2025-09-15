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
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class AttendanceModel : Attendance
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class BranchModel : Branch
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class CustomerModel : Customer
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class DepartmentModel : Department
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class EmployeeModel : Employee
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class FeedbackModel : Feedback
    {
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
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class InvoiceDetailModel : InvoiceDetail
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class LeaveRequestModel : LeaveRequest
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class HistoryMailModel : HistoryMail
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class HistoryNotificationModel : HistoryNotification
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class TemplateMailModel : TemplateMail
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class TemplateNotificationModel : TemplateNotification
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class PositionModel : Position
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class ProductModel : Product
    {
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
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class PromotionModel : Promotion
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class RoleModel : Role
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SalaryModel : Salary
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class ScheduleModel : Schedule
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SpaServiceModel : SpaService
    {
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
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SupplierModel : Supplier
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SupplierOrderModel : SupplierOrder
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class SupplierOrderDetailModel : SupplierOrderDetail
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class TrainingSessionModel : TrainingSession
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    public partial class UserModel : User
    {
        public bool Remember { get; set; } = false;
        public string Password { get; set; } = string.Empty;
        [ViewField]
        public string EmployeeName { get; set; } = string.Empty;


        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
        public string Ids { get; set; } = string.Empty;
        public bool AllowPaging { get; set; } = true;
    }

    #endregion

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
        public string[]? Labels { get; set; }   // Có thể null nếu không dùng
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
}
