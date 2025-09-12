using LazZiya.ExpressLocalization;
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
builder.Services.AddScoped<IDashboardAPIService, DashboardAPIService>();
builder.Services.AddScoped<IInvoiceDetailAPIService, InvoiceDetailAPIService>();
builder.Services.AddScoped<IInvoiceAPIService, InvoiceAPIService>();
builder.Services.AddScoped<ILeaveRequestAPIService, LeaveRequestAPIService>();
builder.Services.AddScoped<INotificationAPIService, NotificationAPIService>();
builder.Services.AddScoped<IMailAPIService, MailAPIService>();
builder.Services.AddScoped<ITemplateMailAPIService, TemplateMailAPIService>();
builder.Services.AddScoped<ITemplateNotificationAPIService, TemplateNotificationAPIService>();
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

var cultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("vi"),
};

builder.Services.AddControllersWithViews()
    .AddExpressLocalization<ExpressLocalizationResources, ViewLocalizationResources>(ops =>
{
    ops.UseAllCultureProviders = true;
    ops.ResourcesPath = "LocalizationResources";
    ops.RequestLocalizationOptions = o =>
    {
        o.SupportedCultures = cultures;
        o.SupportedUICultures = cultures;
        o.DefaultRequestCulture = new RequestCulture("vi");
    };
});
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

#endregion

#region Cookie Auth

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
{
    options.LoginPath = "/vi/Account/AccountIndex";
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
    pattern: "{culture=vi}/{controller=Dashboard}/{action=DashboardIndex}/{id?}"
);
app.Run();
