using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NextERP.ModelBase;
using NextERP.Util;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NextERP.MVC.Admin.Controllers
{
    public class BaseController : Controller
    {
        #region Infrastructure

        private readonly IConfiguration _configuration;
        private readonly ISharedCultureLocalizer _localizer;

        public BaseController(IConfiguration configuration, ISharedCultureLocalizer localizer)
        {
            _configuration = configuration;
            _localizer = localizer;
        }

        #endregion

        #region Default Operations

        #endregion

        #region Custom Operations

        #region Authentication check

        /// <summary>
        /// Phương thức được gọi sau khi một action trong Controller thực thi xong.
        /// Mục đích: kiểm tra token phiên đăng nhập từ cookie và xử lý trạng thái phiên.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var infor = GetInfor();

            if(infor != null && infor.Count() > 0)
            {
                ViewBag.UserId = infor[0];
                ViewBag.OwnerId = infor[1];
            }

            // Lấy token từ cookie
            var cookiesToken = HttpContext.Request.Cookies[Constants.Token];
            if (string.IsNullOrWhiteSpace(cookiesToken))
            {
                // Xóa Authentication cookie
                context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).GetAwaiter().GetResult();

                // Nếu không có thông tin phiên, Redirect đến trang Index trong Controller Account
                context.Result = new RedirectToActionResult(ScreenName.AccountIndex, "Account", null);
            }
            else
            {
                var userPrincipal = ValidateToken(DataHelper.GetString(cookiesToken));
                var expClaim = userPrincipal?.FindFirst("exp");
                if (expClaim != null && long.TryParse(expClaim.Value, out var expTimestamp))
                {
                    var expirationDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expTimestamp);
                    if (expirationDateTimeOffset <= DateTimeOffset.UtcNow)
                    {
                        // Nếu phiên đã hết hạn, xóa cookie
                        HttpContext.Response.Cookies.Delete(Constants.Token);

                        // Redirect đến trang Index trong Controller Account
                        context.Result = new RedirectToActionResult(ScreenName.AccountIndex, "Account", null);
                    }
                }
            }

            // Gọi base.OnActionExecuted để thực hiện các hành động khác nếu cần
            base.OnActionExecuted(context);
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            var tokenKey = _configuration["Tokens:Key"];
            if (string.IsNullOrEmpty(tokenKey))
            {
                throw new ArgumentNullException(nameof(tokenKey), "Token key cannot be null or empty.");
            }

            var validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidAudience = _configuration["Tokens:Issuer"],
                ValidIssuer = _configuration["Tokens:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey))
            };

            return new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out _);
        }

        #endregion

        #region Get Information user login

        public List<string> GetInfor()
        {
            var listInfor = new List<string>();

            var cookiesToken = HttpContext.Request.Cookies[Constants.Token];
            var nameIdentifierClaim = string.Empty;

            if (!string.IsNullOrEmpty(cookiesToken))
            {
                var userPrincipal = ValidateToken(cookiesToken);

                var userIdClaim = userPrincipal.Claims.Where(s => s.Type == "UserID").Select(s => s.Value.ToString()).FirstOrDefault();
                var ownerIdClaim = userPrincipal.Claims.Where(s => s.Type == "OwnerID").Select(s => s.Value.ToString()).FirstOrDefault();

                if (userIdClaim != null)
                    listInfor.Add(userIdClaim);

                if (ownerIdClaim != null)
                    listInfor.Add(ownerIdClaim);
            }

            return listInfor;
        }

        #endregion

        #region ModelState Error Handling and Localization

        protected JsonResult GetModelStateErrors()
        {
            var errors = ModelState
                .Where(x => x.Value!.Errors.Any())
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

        #endregion

        #endregion
    }
}
