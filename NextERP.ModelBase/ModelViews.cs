﻿using Microsoft.AspNetCore.Http;
using NextERP.DAL.Models;

namespace NextERP.ModelBase
{
    #region Model Database

    public partial class AppointmentModel : Appointment
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class AttendanceModel : Attendance
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class BranchModel : Branch
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class CustomerModel : Customer
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class DepartmentModel : Department
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class EmployeeModel : Employee
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class FeedbackModel : Feedback
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class FunctionModel : Function
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class InvoiceModel : Invoice
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class InvoiceDetailModel : InvoiceDetail
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class LeaveRequestModel : LeaveRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class NotificationModel : Notification
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class PositionModel : Position
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class ProductModel : Product
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class ProductCategoryModel : ProductCategory
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class PromotionModel : Promotion
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class RoleModel : Role
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class SalaryModel : Salary
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class ScheduleModel : Schedule
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class SpaServiceModel : Service
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class SpaServiceCategoryModel : ServiceCategory
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class SupplierModel : Supplier
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class SupplierOrderModel : SupplierOrder
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class SupplierOrderDetailModel : SupplierOrderDetail
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class TrainingSessionModel : TrainingSession
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public partial class UserModel : User
    {
        public bool Remember { get; set; }
        public string Password { get; set; } = string.Empty;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    #endregion

    public class Filter
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? KeyWord { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
    }
}
