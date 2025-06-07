using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.Util;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NextERP.MVC.Admin.Controllers
{
    public class BaseController : Controller
    {
        private readonly IConfiguration _configuration;

        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Authentication check

        /// <summary>
        /// Phương thức được gọi sau khi một action trong Controller thực thi xong.
        /// Mục đích: kiểm tra token phiên đăng nhập từ cookie và xử lý trạng thái phiên.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Lấy token từ cookie
            var cookiesToken = HttpContext.Request.Cookies[Constants.Token];
            if (!string.IsNullOrEmpty(cookiesToken))
            {
                // Nếu không có thông tin phiên, Redirect đến trang Index trong Controller Account
                context.Result = new RedirectToActionResult(Constants.Index, "Account", null);
            }
            else
            {
                var userPrincipal = this.ValidateToken(DataHelper.GetString(cookiesToken));
                var expClaim = userPrincipal?.FindFirst("exp");
                if (expClaim != null && long.TryParse(expClaim.Value, out var expTimestamp))
                {
                    var expirationDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expTimestamp);
                    if (expirationDateTimeOffset <= DateTimeOffset.UtcNow)
                    {
                        // Nếu phiên đã hết hạn, xóa cookie
                        HttpContext.Response.Cookies.Delete(Constants.Token);

                        // Redirect đến trang Index trong Controller Account
                        context.Result = new RedirectToActionResult(Constants.Index, "Account", null);
                    }
                }
            }

            // Gọi base.OnActionExecuted để thực hiện các hành động khác nếu cần
            base.OnActionExecuted(context);
        }

        public ClaimsPrincipal ValidateToken(string jwtToken)
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

        public string GetInfor()
        {
            var cookiesToken = HttpContext.Request.Cookies[Constants.Token];
            var nameIdentifierClaim = string.Empty;

            if (!string.IsNullOrEmpty(cookiesToken))
            {
                var userPrincipal = this.ValidateToken(cookiesToken);

                nameIdentifierClaim = userPrincipal.Claims
                    .Where(s => s.Type == ClaimTypes.NameIdentifier)
                    .Select(s => s.Value.ToString()).FirstOrDefault() ?? string.Empty;
            }

            return nameIdentifierClaim;
        }

        #endregion
    }
}
