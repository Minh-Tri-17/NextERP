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
    public class SalaryController : ControllerBase
    {
        #region Infrastructure

        private readonly ISalaryService _salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditSalary))]
        public async Task<ActionResult<Salary>> CreateOrEditSalary()
        {
            var salary = new SalaryModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    salary = JsonConvert.DeserializeObject<SalaryModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (salary != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    salary.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    salary = JsonConvert.DeserializeObject<SalaryModel>(body);
            }

            if (salary == null)
                return BadRequest();

            var result = await _salaryService.CreateOrEdit(salary);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteSalary))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteSalary(string ids)
        {
            var result = await _salaryService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlySalary))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlySalary(string ids)
        {
            var result = await _salaryService.DeletePermanently(ids);
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

        [HttpPost($"{nameof(GetSalaries)}/Filter")]
        public async Task<ActionResult<IEnumerable<Salary>>> GetSalaries()
        {
            var salary = new SalaryModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    salary = JsonConvert.DeserializeObject<SalaryModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (salary != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    salary.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    salary = JsonConvert.DeserializeObject<SalaryModel>(body);
            }

            if (salary == null)
                return BadRequest();

            var result = await _salaryService.GetPaging(salary);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportSalary))]
        public async Task<ActionResult<Salary>> ImportSalary()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

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
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportSalary()
        {
            var salary = new SalaryModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    salary = JsonConvert.DeserializeObject<SalaryModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (salary != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    salary.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    salary = JsonConvert.DeserializeObject<SalaryModel>(body);
            }

            if (salary == null)
                return BadRequest();

            var result = await _salaryService.Export(salary);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
