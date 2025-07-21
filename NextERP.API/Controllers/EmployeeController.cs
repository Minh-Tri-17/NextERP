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
    public class EmployeeController : ControllerBase
    {
        #region Infrastructure

        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditEmployee))]
        public async Task<ActionResult<Employee>> CreateOrEditEmployee()
        {
            var employee = new EmployeeModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    employee = JsonConvert.DeserializeObject<EmployeeModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (employee != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    employee.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    employee = JsonConvert.DeserializeObject<EmployeeModel>(body);
            }

            if (employee == null)
                return BadRequest();

            var result = await _employeeService.CreateOrEdit(employee);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteEmployee))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteEmployee(string ids)
        {
            var result = await _employeeService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyEmployee))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyEmployee(string ids)
        {
            var result = await _employeeService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetEmployee)}/{{id}}")]
        public async Task<ActionResult<Employee>> GetEmployee(Guid id)
        {
            var result = await _employeeService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetEmployees)}/Filter")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(Filter filter)
        {
            var result = await _employeeService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportEmployee))]
        public async Task<ActionResult<Employee>> ImportEmployee()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _employeeService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportEmployee))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportEmployee(Filter filter)
        {
            var result = await _employeeService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
