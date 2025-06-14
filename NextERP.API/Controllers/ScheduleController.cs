using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NextERP.BLL.Interface;
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
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost(nameof(CreateOrEditSchedule))]
        public async Task<ActionResult<Schedule>> CreateOrEditSchedule()
        {
            var schedule = new ScheduleModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    schedule = JsonConvert.DeserializeObject<ScheduleModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (schedule != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    schedule.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    schedule = JsonConvert.DeserializeObject<ScheduleModel>(body);
            }

            if (schedule == null)
                return BadRequest();

            var result = await _scheduleService.CreateOrEdit(schedule);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteSchedule))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteSchedule(string ids)
        {
            var result = await _scheduleService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetSchedule)}/{{id}}")]
        public async Task<ActionResult<Schedule>> GetSchedule(Guid id)
        {
            var result = await _scheduleService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetSchedules)}/Filter")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules(Filter filter)
        {
            var result = await _scheduleService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportSchedule))]
        public async Task<ActionResult<Schedule>> ImportSchedule()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _scheduleService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportSchedule))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportSchedule(Filter filter)
        {
            var result = await _scheduleService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
