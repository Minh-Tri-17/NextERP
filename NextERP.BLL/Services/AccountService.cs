using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly NextErpContext _context;
        private readonly IConfiguration _config;

        public AccountService(NextErpContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<APIBaseResult<string>> Auth(UserModel request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
            if (user == null)
                return new APIErrorResult<string>(string.Format(Messages.UserNameNotExist, request.Username));

            var checkPassword = PasswordHasher.VerifyPassword(request.Password, DataHelper.GetString(user.PasswordHash));
            if (checkPassword == false)
                return new APIErrorResult<string>(string.Format(Messages.UserNameOrPasswordIncorrect, request.Username, request.Password));

            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == user.EmployeeId);
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == user.CustomerId);

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
                .Where(x => x.GroupRole == user.GroupRole &&
                    (isAllRoles || roleIds.Contains(x.Id.ToString())))
                .ToListAsync();

            claims.AddRange(listRole.Select(role => new Claim(ClaimTypes.Role, DataHelper.GetString(role.RoleName))));

            #endregion

            var tokenKey = _config["Tokens:Key"];
            if (string.IsNullOrEmpty(tokenKey))
            {
                throw new InvalidOperationException(Messages.TokenKeyNotConfigured);
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

            return new APISuccessResult<string>(Messages.AuthSuccess, new JwtSecurityTokenHandler().WriteToken(token));
        }

        /// <summary>
        /// Chỉ dùng cho khách hàng đăng ký tài khoản, còn đối với nhân viên thì không đăng ký tài khoản mà sẽ tạo tài khoản
        /// Khi khách hàng đăng ký thì tài khoản chưa có CustomerId
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<APIBaseResult<bool>> Register(UserModel request)
        {
            var checkUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
            if (checkUser != null)
                return new APIErrorResult<bool>(string.Format(Messages.UserNameExist, request.Username));

            // Mặc định gán quyền cho khách hàng là Customer
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleName != null
                && x.RoleName.ToLower() == Enums.GroupRole.Customer.ToString().ToLower());
            if (role == null)
                return new APIErrorResult<bool>(Messages.RoleNotExist);

            var passwordHashed = PasswordHasher.HashPassword(request.Password);
            request.PasswordHash = passwordHashed;
            request.RoleIds = DataHelper.GetString(role.Id.ToString());

            var user = new User();
            DataHelper.MapAudit(request, user, string.Empty);

            await _context.Users.AddAsync(user);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>();

            return new APIErrorResult<bool>();
        }
    }
}
