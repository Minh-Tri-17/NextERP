using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static NextERP.Util.Enums;

namespace NextERP.MVC.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountAPIService _accountAPIService;
        private readonly IRoleAPIService _roleAPIService;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountAPIService accountAPIService, IConfiguration configuration,
            IRoleAPIService roleAPIService)
        {
            _accountAPIService = accountAPIService;
            _configuration = configuration;
            _roleAPIService = roleAPIService;
        }

        [HttpGet]
        public IActionResult AccountIndex()
        {
            // Kiểm tra xem user đã login chưa, nếu đã đăng nhập thì chuyển thẳng tới trang Home
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction(ActionName.DashboardIndex, "Dashboard");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AccountIndex(UserModel request)
        {
            var result = await _accountAPIService.Auth(request);

            if (DataHelper.IsNotNull(result))
            {
                var userPrincipal = ValidateToken(DataHelper.GetString(result.Result));

                if (userPrincipal != null)
                {
                    var expClaim = userPrincipal.FindFirst("exp");
                    if (expClaim == null)
                        return View();

                    if (long.TryParse(expClaim.Value, out var expTimestamp))
                    {
                        var expirationDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expTimestamp);

                        var authProperties = new AuthenticationProperties
                        {
                            ExpiresUtc = expirationDateTimeOffset,
                            IsPersistent = true,
                        };

                        // Lưu Token vào cookie
                        HttpContext.Response.Cookies.Append(Constants.Token, DataHelper.GetString(result.Result), new CookieOptions
                        {
                            HttpOnly = true,        // Tránh JS truy cập (bảo mật)
                            Secure = true,          // Chỉ gửi trên HTTPS
                            SameSite = SameSiteMode.Lax,
                            Expires = expirationDateTimeOffset
                        });

                        // Đăng nhập người dùng với ClaimsPrincipal và AuthenticationProperties
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);
                    }

                    bool isAdmin = request.GroupRole == GroupRole.Admin.ToString();
                    bool isEmployee = request.GroupRole == GroupRole.Employee.ToString();
                    bool isCustomer = request.GroupRole == GroupRole.Customer.ToString();

                    if (isAdmin)
                        return RedirectToAction(ActionName.DashboardIndex, "Dashboard");
                    else if (isEmployee)
                        return View();// RedirectToAction(Constants.Index, "Employee");
                    else if (isCustomer)
                        return View(); // RedirectToAction(Constants.Index, "Customer");
                    else
                        return View();
                }
                else
                {
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = Messages.AuthFailed;
                return View();
            }
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            var validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidAudience = _configuration["Tokens:Issuer"] ?? throw new ArgumentNullException("Tokens:Issuer"),
                ValidIssuer = _configuration["Tokens:Issuer"] ?? throw new ArgumentNullException("Tokens:Issuer"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"] ?? throw new ArgumentNullException("Tokens:Key")))
            };

            return new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out _);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete(Constants.Token);
            return RedirectToAction(ActionName.AccountIndex, "Account");
        }
    }
}
