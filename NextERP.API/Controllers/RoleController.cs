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
    public class RoleController : ControllerBase
    {
        #region Infrastructure

        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditRole))]
        public async Task<ActionResult<Role>> CreateOrEditRole()
        {
            var role = new RoleModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    role = JsonConvert.DeserializeObject<RoleModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (role != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    role.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    role = JsonConvert.DeserializeObject<RoleModel>(body);
            }

            if (role == null)
                return BadRequest();

            var result = await _roleService.CreateOrEdit(role);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteRole))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteRole(string ids)
        {
            var result = await _roleService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyRole))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyRole(string ids)
        {
            var result = await _roleService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetRole)}/{{id}}")]
        public async Task<ActionResult<Role>> GetRole(Guid id)
        {
            var result = await _roleService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetRoles)}/Filter")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles(Filter filter)
        {
            var result = await _roleService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportRole))]
        public async Task<ActionResult<Role>> ImportRole()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _roleService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportRole))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportRole(Filter filter)
        {
            var result = await _roleService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
