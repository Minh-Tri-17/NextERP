using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost(nameof(GetEmployees))]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(Filter filter)
        {
            var result = await _employeeService.GetPaging(filter);
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

        [HttpPost(nameof(CreateOrEditEmployee))]
        public async Task<ActionResult<Employee>> CreateOrEditEmployee([FromBody] EmployeeModel employee)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _employeeService.CreateOrEdit(employee.Id, employee);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteEmployee))]
        public async Task<IActionResult> DeleteEmployee(string ids)
        {
            var result = await _employeeService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportEmployee))]
        public async Task<ActionResult<Employee>> ImportEmployee()
        {
            IFormFile excelFile = Request.Form.Files["ExcelFiles"]!;

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
        public async Task<IActionResult> ExportEmployee(Filter filter)
        {
            var result = await _employeeService.Export(filter);

            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            var fileName = string.Format(Constants.FileName, ObjectNames.Employee, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result, Constants.ContentType, fileName);
        }
    }
}
