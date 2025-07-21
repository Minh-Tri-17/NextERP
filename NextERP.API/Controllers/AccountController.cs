using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.Util;

namespace NextERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Infrastructure

        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        #region Default Operations

        #endregion

        #region Custom Operations

        [HttpPost(nameof(Authentication))]
        public async Task<ActionResult<User>> Authentication()
        {
            var user = new UserModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    user = JsonConvert.DeserializeObject<UserModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (user != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    user.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    user = JsonConvert.DeserializeObject<UserModel>(body);
            }

            if (user == null)
                return BadRequest();

            var result = await _accountService.Auth(user);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(Register))]
        public async Task<ActionResult<User>> Register()
        {
            var user = new UserModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    user = JsonConvert.DeserializeObject<UserModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (user != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    user.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    user = JsonConvert.DeserializeObject<UserModel>(body);
            }

            if (user == null)
                return BadRequest();

            var result = await _accountService.Register(user);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion
    }
}
