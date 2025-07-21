using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextERP.BLL.Interface;
using NextERP.BLL.Service;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Đặt ở đây để toàn bộ API đều cần xác thực
    public class UserController : ControllerBase
    {
        #region Infrastructure

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditUser))]
        public async Task<ActionResult<User>> CreateOrEditUser()
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

            var result = await _userService.CreateOrEdit(user);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteUser))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteUser(string ids)
        {
            var result = await _userService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyUser))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyUser(string ids)
        {
            var result = await _userService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetUser)}/{{id}}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var result = await _userService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetUsers)}/Filter")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(Filter filter)
        {
            var result = await _userService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportUser))]
        public async Task<ActionResult<User>> ImportUser()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _userService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportUser))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportUser(Filter filter)
        {
            var result = await _userService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
