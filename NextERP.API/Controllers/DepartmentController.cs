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
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost(nameof(CreateOrEditDepartment))]
        public async Task<ActionResult<Department>> CreateOrEditDepartment()
        {
            var department = new DepartmentModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    department = JsonConvert.DeserializeObject<DepartmentModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (department != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    department.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    department = JsonConvert.DeserializeObject<DepartmentModel>(body);
            }

            if (department == null)
                return BadRequest();

            var result = await _departmentService.CreateOrEdit(department);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteDepartment))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteDepartment(string ids)
        {
            var result = await _departmentService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyDepartment))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyDepartment(string ids)
        {
            var result = await _departmentService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetDepartment)}/{{id}}")]
        public async Task<ActionResult<Department>> GetDepartment(Guid id)
        {
            var result = await _departmentService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetDepartments)}/Filter")]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments(Filter filter)
        {
            var result = await _departmentService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportDepartment))]
        public async Task<ActionResult<Department>> ImportDepartment()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _departmentService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportDepartment))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportDepartment(Filter filter)
        {
            var result = await _departmentService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
