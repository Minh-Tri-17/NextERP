using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static NextERP.Util.Enums;

namespace NextERP.MVC.Admin.Controllers
{
    public class AccountController : Controller
    {
        #region Infrastructure

        private readonly IAccountAPIService _accountAPIService;
        private readonly IRoleAPIService _roleAPIService;
        private readonly IConfiguration _configuration;
        private readonly ISharedCultureLocalizer _localizer;

        public AccountController(IAccountAPIService accountAPIService, IConfiguration configuration,
            IRoleAPIService roleAPIService, ISharedCultureLocalizer localizer)
        {
            _accountAPIService = accountAPIService;
            _configuration = configuration;
            _roleAPIService = roleAPIService;
            _localizer = localizer;
        }

        #endregion

        #region Default Operations

        #endregion

        #region Custom Operations

        [HttpGet]
        public IActionResult AccountIndex()
        {
            ViewBag.Title = Constants.Login;

            // Kiểm tra xem user đã login chưa, nếu đã đăng nhập thì chuyển thẳng tới trang Home
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction(ScreenName.DashboardIndex, "Dashboard");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AccountIndex(UserModel request)
        {
            var result = await _accountAPIService.Auth(request);

            if (DataHelper.IsNotNull(result) && result.Result != null)
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
                        return RedirectToAction(ScreenName.DashboardIndex, "Dashboard");
                    else if (isEmployee)
                        return RedirectToAction(ScreenName.AccountIndex, Constants.Account);// RedirectToAction(Constants.Index, "Employee");
                    else if (isCustomer)
                        return RedirectToAction(ScreenName.AccountIndex, Constants.Account); // RedirectToAction(Constants.Index, "Customer");
                    else
                        return RedirectToAction(ScreenName.AccountIndex, Constants.Account);
                }
                else
                {
                    return RedirectToAction(ScreenName.AccountIndex, Constants.Account);
                }
            }
            else
            {
                return RedirectToAction(ScreenName.AccountIndex, Constants.Account);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete(Constants.Token);
            return RedirectToAction(ScreenName.AccountIndex, Constants.Account);
        }

        [HttpPost]
        public async Task<ActionResult> SendOTP(MailModel mail)
        {
            if (mail.To == null)
                return GetModelStateErrors();

            var otpString = GenerateOtp(mail.To);

            mail.Subject = _localizer.GetLocalizedString(Constants.SendOTP);
            mail.Body = _localizer.GetLocalizedString(Messages.YourOTP, new object[] { otpString });

            var result = await _accountAPIService.SendOTP(mail);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(UserModel request)
        {
            var validateMessage = ValidateOtp(DataHelper.GetString(request.Mail), DataHelper.GetString(request.Otp));

            if (!string.IsNullOrEmpty(validateMessage))
                return Json(_localizer.GetLocalizedString(validateMessage));

            var result = await _accountAPIService.ResetPassword(request);

            if (!DataHelper.IsNotNull(result))
            {
                var errors = result.Message.ToString().Split(',')
                     .Select(errorItem =>
                     {
                         var parts = errorItem.Split('|');
                         var code = parts[0].Trim();
                         var args = parts.Skip(1).ToArray();

                         return new
                         {
                             Field = UserModel.AttributeNames.Password,
                             Message = $"<b>&#10031; [{UserModel.AttributeNames.Password}]</b> {_localizer.GetLocalizedString(code, args)}"
                         };
                     })
                    .ToList();

                return Json(errors);
            }

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        #region Private Methods

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

        private static readonly ConcurrentDictionary<string, object> otpStore
            = new ConcurrentDictionary<string, object>();

        private JsonResult GetModelStateErrors()
        {
            var errors = ModelState
                .Where(x => x.Value!.Errors.Any() && x.Key != "Body" && x.Key != "Subject")
                .SelectMany(x => x.Value!.Errors, (entry, error) =>
                {
                    var field = entry.Key;
                    // Phân tích loại lỗi từ nội dung thông báo
                    var errorType = GetErrorType(error.ErrorMessage);

                    string message = $"<b>&#10031; [{_localizer.GetLocalizedString(field)}]</b> {_localizer.GetLocalizedString(errorType)}";

                    return new { Field = field, Message = message };
                })
                .ToList();

            return Json(errors);
        }

        private static string GetErrorType(string message)
        {
            if (message.Contains("required", StringComparison.OrdinalIgnoreCase))
                return "Required";
            if (message.Contains("valid e-mail", StringComparison.OrdinalIgnoreCase))
                return "Regex";
            if (message.Contains("is invalid", StringComparison.OrdinalIgnoreCase))
                return "Invalid";

            return "Other";
        }

        private string ValidateOtp(string email, string otp)
        {
            if (!otpStore.TryGetValue(email, out var entryObj))
                return Messages.OTPNotFound;

            var entry = (dynamic)entryObj;

            // Kiểm tra hết hạn
            if (DateTime.UtcNow > entry.ExpiresAt)
            {
                otpStore.TryRemove(email, out _);
                return Messages.OTPExpired;
            }

            // Kiểm tra số lần thử còn lại
            if (entry.AttemptsLeft <= 0)
            {
                otpStore.TryRemove(email, out _);
                return Messages.OTPNoAttemptsLeft;
            }

            // Hash OTP nhập để so sánh
            using var sha = SHA256.Create();
            var hashInput = sha.ComputeHash(Encoding.UTF8.GetBytes(otp + entry.Salt));
            var hashInputString = Convert.ToBase64String(hashInput);

            if (hashInputString != entry.Hashed)
            {
                entry.AttemptsLeft--;
                otpStore[email] = entry; // cập nhật số lần thử
                return Messages.OTPIncorrect;
            }

            // OTP đúng → remove khỏi store
            otpStore.TryRemove(email, out _);

            return string.Empty;
        }

        private string GenerateOtp(string email)
        {
            // 1. Sinh OTP 6 chữ số an toàn
            byte[] rngBytes = new byte[4];
            RandomNumberGenerator.Fill(rngBytes);
            var otp = BitConverter.ToUInt32(rngBytes, 0) % 1000000;
            var otpString = otp.ToString("D6");

            // 2. Tạo salt + hash OTP
            var salt = Guid.NewGuid().ToString();
            using var sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(otpString + salt));
            var hashed = Convert.ToBase64String(hashBytes);

            // 3. Lưu vào store tạm
            otpStore[email] = new
            {
                Hashed = hashed,
                Salt = salt,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                AttemptsLeft = 3
            };

            return otpString;
        }

        #endregion

        #endregion
    }
}
