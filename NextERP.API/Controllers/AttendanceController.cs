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
    public class AttendanceController : ControllerBase
    {
        #region Infrastructure

        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditAttendance))]
        public async Task<ActionResult<Attendance>> CreateOrEditAttendance()
        {
            var attendance = new AttendanceModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    attendance = JsonConvert.DeserializeObject<AttendanceModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (attendance != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    attendance.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    attendance = JsonConvert.DeserializeObject<AttendanceModel>(body);
            }

            if (attendance == null)
                return BadRequest();

            var result = await _attendanceService.CreateOrEdit(attendance);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteAttendance))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteAttendance(string ids)
        {
            var result = await _attendanceService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyAttendance))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyAttendance(string ids)
        {
            var result = await _attendanceService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetAttendance)}/{{id}}")]
        public async Task<ActionResult<Attendance>> GetAttendance(Guid id)
        {
            var result = await _attendanceService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetAttendances)}/Filter")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendances()
        {
            var attendance = new AttendanceModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    attendance = JsonConvert.DeserializeObject<AttendanceModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (attendance != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    attendance.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    attendance = JsonConvert.DeserializeObject<AttendanceModel>(body);
            }

            if (attendance == null)
                return BadRequest();

            var result = await _attendanceService.GetPaging(attendance);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportAttendance))]
        public async Task<ActionResult<Attendance>> ImportAttendance()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _attendanceService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportAttendance))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportAttendance()
        {
            var attendance = new AttendanceModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    attendance = JsonConvert.DeserializeObject<AttendanceModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (attendance != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    attendance.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    attendance = JsonConvert.DeserializeObject<AttendanceModel>(body);
            }

            if (attendance == null)
                return BadRequest();

            var result = await _attendanceService.Export(attendance);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
