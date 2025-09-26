using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.Util;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NextERP.BLL.Service
{
    public class AccountService : IAccountService
    {
        #region Infrastructure

        private readonly NextErpContext _context;
        private readonly IConfiguration _config;
        private readonly IdentityOptions _options;

        public AccountService(NextErpContext context, IConfiguration config, IOptions<IdentityOptions> options)
        {
            _context = context;
            _config = config;
            _options = options.Value;
        }

        #endregion

        #region Default Operations

        #endregion

        #region Custom Operations

        public async Task<APIBaseResult<string>> Auth(UserModel request)
        {
            #region Check null request and create variable

            var username = DataHelper.GetString(request.Username);
            var password = DataHelper.GetString(request.Password);

            #endregion

            var user = await _context.Users.FirstOrDefaultAsync(s => !string.IsNullOrWhiteSpace(s.Username) && s.Username == username);
            if (user == null)
                return new APIErrorResult<string>(string.Format(Messages.UserNameNotExist, username));

            var checkPassword = PasswordHasher.VerifyPassword(password, DataHelper.GetString(user.PasswordHash));
            if (checkPassword == false)
                return new APIErrorResult<string>(string.Format(Messages.UserNameOrPasswordIncorrect, username, password));

            var employee = await _context.Employees.FirstOrDefaultAsync(s => s.Id == DataHelper.GetGuid(user.EmployeeId));
            var customer = await _context.Customers.FirstOrDefaultAsync(s => s.Id == DataHelper.GetGuid(user.CustomerId));

            var id = employee != null ? employee.Id : customer != null ? customer.Id : Guid.Empty;
            var fullName = employee != null ? employee.FullName : customer != null ? customer.FullName : string.Empty;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, DataHelper.GetString(user.Username)),
                new Claim(ClaimTypes.GivenName, DataHelper.GetString(fullName)),
                new Claim(ClaimTypes.NameIdentifier, DataHelper.GetString(id.ToString())),
            };

            #region Add roleNames to claim Role

            var roleIds = DataHelper.GetString(user.RoleIds);
            bool isAllRoles = roleIds.Contains(Guid.Empty.ToString());

            var listRole = await _context.Roles
                .Where(s => s.GroupRole == user.GroupRole &&
                    (isAllRoles || roleIds.Contains(s.Id.ToString())))
                .ToListAsync();

            claims.AddRange(listRole.Select(role => new Claim(ClaimTypes.Role, DataHelper.GetString(role.RoleName))));

            #endregion

            var tokenKey = _config["Tokens:Key"];
            if (string.IsNullOrEmpty(tokenKey))
            {
                return new APIErrorResult<string>(Messages.TokenKeyNotConfigured);
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime expirationTime;
            expirationTime = request.Remember ? DateTime.Now.AddYears(1) : DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
               _config["Tokens:Issuer"],
               claims,
               expires: expirationTime,
               signingCredentials: creds);

            if (token != null)
                return new APISuccessResult<string>(Messages.AuthSuccess, new JwtSecurityTokenHandler().WriteToken(token));

            return new APIErrorResult<string>(Messages.AuthSuccess);
        }

        /// <summary>
        /// Chỉ dùng cho khách hàng đăng ký tài khoản, còn đối với nhân viên thì không đăng ký tài khoản mà sẽ tạo tài khoản
        /// Khi khách hàng đăng ký thì tài khoản chưa có CustomerId
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<APIBaseResult<bool>> Register(UserModel request)
        {
            #region Check null request and create variable

            var username = DataHelper.GetString(request.Username);
            var password = DataHelper.GetString(request.Password);

            #endregion

            var checkUser = await _context.Users.FirstOrDefaultAsync(s => !string.IsNullOrWhiteSpace(s.Username)
                && s.Username == username);

            if (checkUser != null)
                return new APIErrorResult<bool>(string.Format(Messages.UserNameExist, username));

            // Mặc định gán quyền cho khách hàng là Customer
            var listRole = await _context.Roles.Where(s => !string.IsNullOrEmpty(s.GroupRole)
                && s.GroupRole.Contains(Enums.GroupRole.Customer.ToString())).Select(s => s.RoleName).ToListAsync();
            if (listRole == null || listRole.Count() == 0)
                return new APIErrorResult<bool>(Messages.RoleNotExist);

            var passwordHashed = PasswordHasher.HashPassword(password);
            request.PasswordHash = passwordHashed;
            request.RoleIds = string.Join(";", listRole);

            var user = new User();
            DataHelper.MapAudit(request, user, string.Empty);

            await _context.Users.AddAsync(user);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.RegisterSuccess, true);

            return new APIErrorResult<bool>(Messages.RegisterFailed);
        }

        public async Task<APIBaseResult<bool>> SendOTP(MailModel mail)
        {
            var result = await MailHelper.SendMail(mail);
            if (result)
                return new APISuccessResult<bool>(Messages.SendMailSuccess, true);

            return new APIErrorResult<bool>(Messages.SendMailFailed);
        }

        public async Task<APIBaseResult<bool>> ResetPassword(UserModel request)
        {
            #region Check null request and create variable

            var username = DataHelper.GetString(request.Username);
            var phoneNumber = DataHelper.GetString(request.PhoneNumber);
            var mail = DataHelper.GetString(request.Mail);
            var password = DataHelper.GetString(request.Password);
            var passwordHashed = PasswordHasher.HashPassword(password);

            #endregion

            var user = await _context.Users.FirstOrDefaultAsync(s => s.Username == username
                && s.PhoneNumber == phoneNumber && s.Mail == mail);

            if (user == null)
                return new APIErrorResult<bool>(Messages.NotFoundUpdate);

            var validationResult = ValidatePassword(password);
            if (validationResult.Any())
            {
                var error = string.Join(", ", validationResult.Select(e =>
                    e.Description != null && e.Description.Length > 0
                    ? $"{e.Code}|{string.Join("|", e.Description)}" : e.Code));

                return new APIErrorResult<bool>(error);
            }

            user.PasswordHash = passwordHashed;

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ResetPasswordSuccess, true);

            return new APIErrorResult<bool>(Messages.ResetPasswordFailed);
        }

        public List<IdentityError> ValidatePassword(string password)
        {
            var errors = new List<IdentityError>();

            if (password.Length < _options.Password.RequiredLength)
                errors.Add(new IdentityError { Code = Messages.RequiredLength, Description = _options.Password.RequiredLength.ToString() });

            if (_options.Password.RequireDigit && !password.Any(char.IsDigit))
                errors.Add(new IdentityError { Code = Messages.RequireDigit });

            if (_options.Password.RequireLowercase && !password.Any(char.IsLower))
                errors.Add(new IdentityError { Code = Messages.RequireLowercase });

            if (_options.Password.RequireUppercase && !password.Any(char.IsUpper))
                errors.Add(new IdentityError { Code = Messages.RequireUppercase });

            if (_options.Password.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
                errors.Add(new IdentityError { Code = Messages.RequireNonAlphanumeric });

            if (password.Distinct().Count() < _options.Password.RequiredUniqueChars)
                errors.Add(new IdentityError { Code = Messages.RequiredUniqueChars, Description = _options.Password.RequiredUniqueChars.ToString() });

            return errors;
        }

        #endregion
    }
}
