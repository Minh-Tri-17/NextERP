using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;

namespace NextERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost(nameof(Authentication))]
        public async Task<ActionResult<User>> Authentication([FromBody] UserModel user)
        {
            var result = await _accountService.Auth(user);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(Register))]
        public async Task<ActionResult<User>> Register([FromBody] UserModel user)
        {
            var result = await _accountService.Register(user);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
