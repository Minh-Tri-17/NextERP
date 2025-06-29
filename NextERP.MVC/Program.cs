﻿using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using NextERP.MVC.Admin.LocalizationResources;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region DI

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAccountAPIService, AccountAPIService>();
builder.Services.AddScoped<IAppointmentAPIService, AppointmentAPIService>();
builder.Services.AddScoped<IAttendanceAPIService, AttendanceAPIService>();
builder.Services.AddScoped<IBranchAPIService, BranchAPIService>();
builder.Services.AddScoped<ICustomerAPIService, CustomerAPIService>();
builder.Services.AddScoped<IDepartmentAPIService, DepartmentAPIService>();
builder.Services.AddScoped<IEmployeeAPIService, EmployeeAPIService>();
builder.Services.AddScoped<IFeedbackAPIService, FeedbackAPIService>();
builder.Services.AddScoped<IFunctionAPIService, FunctionAPIService>();
builder.Services.AddScoped<IAdminAPIService, AdminAPIService>();
builder.Services.AddScoped<IInvoiceDetailAPIService, InvoiceDetailAPIService>();
builder.Services.AddScoped<IInvoiceAPIService, InvoiceAPIService>();
builder.Services.AddScoped<ILeaveRequestAPIService, LeaveRequestAPIService>();
builder.Services.AddScoped<INotificationAPIService, NotificationAPIService>();
builder.Services.AddScoped<IPositionAPIService, PositionAPIService>();
builder.Services.AddScoped<IProductAPIService, ProductAPIService>();
builder.Services.AddScoped<IProductCategoryAPIService, ProductCategoryAPIService>();
builder.Services.AddScoped<IPromotionAPIService, PromotionAPIService>();
builder.Services.AddScoped<IRoleAPIService, RoleAPIService>();
builder.Services.AddScoped<ISalaryAPIService, SalaryAPIService>();
builder.Services.AddScoped<IScheduleAPIService, ScheduleAPIService>();
builder.Services.AddScoped<ISpaServiceCategoryAPIService, SpaServiceCategoryAPIService>();
builder.Services.AddScoped<ISpaServiceAPIService, SpaServiceAPIService>();
builder.Services.AddScoped<ISupplierOrderDetailAPIService, SupplierOrderDetailAPIService>();
builder.Services.AddScoped<ISupplierOrderAPIService, SupplierOrderAPIService>();
builder.Services.AddScoped<ISupplierAPIService, SupplierAPIService>();
builder.Services.AddScoped<ITrainingSessionAPIService, TrainingSessionAPIService>();
builder.Services.AddScoped<IUserAPIService, UserAPIService>();


#endregion

#region Base 

builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

var cultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("vi"),
};

builder.Services.AddControllersWithViews()
.AddExpressLocalization<ExpressLocalizationResources, ViewLocalizationResources>(ops =>
{
    ops.UseAllCultureProviders = false;
    ops.ResourcesPath = "LocalizationResources";
    ops.RequestLocalizationOptions = o =>
    {
        o.SupportedCultures = cultures;
        o.SupportedUICultures = cultures;
        o.DefaultRequestCulture = new RequestCulture("en");
    };
});

#endregion

#region Cookie Auth

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/vi/Account/Index";
    options.AccessDeniedPath = "/User/Forbidden/";
    options.Cookie.Name = "Cookies";
    options.Cookie.Path = "/";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.SlidingExpiration = true;
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseRequestLocalization();// 🌐 Kích hoạt đa ngôn ngữ cho ứng dụng
app.UseHttpsRedirection();// 🔐 Chuyển hướng HTTPS
app.UseStaticFiles();// 📁 Dùng cho wwwroot (CSS/JS/images)
app.UseRouting();// 🧭 Bắt đầu định tuyến
app.UseAuthentication();// 🔑 Xác thực
app.UseAuthorization();// 🔐 Phân quyền
app.MapControllerRoute(
    name: "default",
    pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}"
);
app.Run();
