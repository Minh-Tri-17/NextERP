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
    public class FunctionController : ControllerBase
    {
        #region Infrastructure

        private readonly IFunctionService _functionService;

        public FunctionController(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditFunction))]
        public async Task<ActionResult<Function>> CreateOrEditFunction()
        {
            var function = new FunctionModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    function = JsonConvert.DeserializeObject<FunctionModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (function != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    function.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    function = JsonConvert.DeserializeObject<FunctionModel>(body);
            }

            if (function == null)
                return BadRequest();

            var result = await _functionService.CreateOrEdit(function);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteFunction))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteFunction(string ids)
        {
            var result = await _functionService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyFunction))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyFunction(string ids)
        {
            var result = await _functionService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetFunction)}/{{id}}")]
        public async Task<ActionResult<Function>> GetFunction(Guid id)
        {
            var result = await _functionService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetFunctions)}/Filter")]
        public async Task<ActionResult<IEnumerable<Function>>> GetFunctions()
        {
            var function = new FunctionModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    function = JsonConvert.DeserializeObject<FunctionModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (function != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    function.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    function = JsonConvert.DeserializeObject<FunctionModel>(body);
            }

            if (function == null)
                return BadRequest();

            var result = await _functionService.GetPaging(function);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
