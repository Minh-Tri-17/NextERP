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
            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Appointment, AppointmentModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.AppointmentCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Attendance, AttendanceModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.AttendanceCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Branch, BranchModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.BranchCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Customer, CustomerModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.CustomerCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Department, DepartmentModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DepartmentCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Employee, EmployeeModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.EmployeeCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 2 field)
            CreateMap<Feedback, FeedbackModel>().ReverseMap()
                .ForMember(dest => dest.FeedbackCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Function, FunctionModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.FunctionCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Invoice, InvoiceModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.InvoiceCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 3 field)
            CreateMap<InvoiceDetail, InvoiceDetailModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.InvoiceDetailCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<LeaveRequest, LeaveRequestModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.LeaveRequestCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Notification, NotificationModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.NotificationCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<TemplateNotification, TemplateNotificationModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.TemplateNotificationCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Mail, MailModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.MailCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<TemplateMail, TemplateMailModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.TemplateMailCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Position, PositionModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.PositionCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Product, ProductModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.ProductCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 3 field)
            CreateMap<ProductImage, ProductImageModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.ProductImageCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<ProductCategory, ProductCategoryModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.ProductCategoryCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Promotion, PromotionModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.PromotionCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Role, RoleModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.RoleCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Salary, SalaryModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.SalaryCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Schedule, ScheduleModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.ScheduleCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<SpaService, SpaServiceModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.SpaServiceCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 3 field)
            CreateMap<SpaServiceImage, SpaServiceImageModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.SpaServiceImageCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<SpaServiceCategory, SpaServiceCategoryModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.SpaServiceCategoryCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<Supplier, SupplierModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.SupplierCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<SupplierOrder, SupplierOrderModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.SupplierOrderCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 3 field)
            CreateMap<SupplierOrderDetail, SupplierOrderDetailModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.SupplierOrderDetailCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<TrainingSession, TrainingSessionModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.TrainingSessionCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));

            // Khi cập nhật (map từ model → entity nhưng bỏ 4 field)
            CreateMap<User, UserModel>().ReverseMap()
                .ForMember(dest => dest.UserCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.DateCreate, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.UserCode, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true))
                .ForMember(dest => dest.IsDelete, opt =>
                    opt.Condition((src, dest, srcMember, destMember, context) =>
                    !context.Items.TryGetValue("IgnoreAuditFields", out var ignore) || ignore is not true));
        }
    }
}
