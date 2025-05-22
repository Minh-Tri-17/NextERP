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
    public class SalariesController : ControllerBase
    {
        private readonly ISalaryService _salaryService;

        public SalariesController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        [HttpPost(nameof(GetSalaries))]
        public async Task<ActionResult<IEnumerable<Salary>>> GetSalaries(Filter filter)
        {
            var result = await _salaryService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetSalary)}/{{id}}")]
        public async Task<ActionResult<Salary>> GetSalary(Guid id)
        {
            var result = await _salaryService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(CreateOrEditSalary))]
        public async Task<ActionResult<Salary>> CreateOrEditSalary([FromBody] SalaryModel salary)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _salaryService.CreateOrEdit(salary.Id, salary);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteSalary))]
        public async Task<IActionResult> DeleteSalary(string ids)
        {
            var result = await _salaryService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportSalary))]
        public async Task<ActionResult<Salary>> ImportSalary()
        {
            IFormFile excelFile = Request.Form.Files["ExcelFiles"]!;

            if (excelFile != null)
            {
                var result = await _salaryService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportSalary))]
        public async Task<IActionResult> ExportSalary(Filter filter)
        {
            var result = await _salaryService.Export(filter);

            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            var fileName = string.Format(Constants.FileName, ObjectNames.Salary, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result, Constants.ContentType, fileName);
        }
    }
}
