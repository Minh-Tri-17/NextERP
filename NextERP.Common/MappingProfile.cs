using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.Util
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Appointment, AppointmentModel>().ReverseMap();
            CreateMap<Attendance, AttendanceModel>().ReverseMap();
            CreateMap<Branch, BranchModel>().ReverseMap();
            CreateMap<Customer, CustomerModel>().ReverseMap();
            CreateMap<Department, DepartmentModel>().ReverseMap();
            CreateMap<Employee, EmployeeModel>().ReverseMap();
            CreateMap<Feedback, FeedbackModel>().ReverseMap();
            CreateMap<Function, FunctionModel>().ReverseMap();
            CreateMap<Invoice, InvoiceModel>().ReverseMap();
            CreateMap<InvoiceDetail, InvoiceDetailModel>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestModel>().ReverseMap();
            CreateMap<Notification, NotificationModel>().ReverseMap();
            CreateMap<Position, PositionModel>().ReverseMap();
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryModel>().ReverseMap();
            CreateMap<Promotion, PromotionModel>().ReverseMap();
            CreateMap<Role, RoleModel>().ReverseMap();
            CreateMap<Salary, SalaryModel>().ReverseMap();
            CreateMap<Schedule, ScheduleModel>().ReverseMap();
            CreateMap<SpaService, SpaServiceModel>().ReverseMap();
            CreateMap<SpaServiceCategory, SpaServiceCategoryModel>().ReverseMap();
            CreateMap<Supplier, SupplierModel>().ReverseMap();
            CreateMap<SupplierOrder, SupplierOrderModel>().ReverseMap();
            CreateMap<SupplierOrderDetail, SupplierOrderDetailModel>().ReverseMap();
            CreateMap<TrainingSession, TrainingSessionModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
