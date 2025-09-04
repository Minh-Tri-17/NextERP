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
    public class AppointmentController : ControllerBase
    {
        #region Infrastructure

        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditAppointment))]
        public async Task<ActionResult<Appointment>> CreateOrEditAppointment()
        {
            var appointment = new AppointmentModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    appointment = JsonConvert.DeserializeObject<AppointmentModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (appointment != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    appointment.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    appointment = JsonConvert.DeserializeObject<AppointmentModel>(body);
            }

            if (appointment == null)
                return BadRequest();

            var result = await _appointmentService.CreateOrEdit(appointment);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteAppointment))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteAppointment(string ids)
        {
            var result = await _appointmentService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyAppointment))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyAppointment(string ids)
        {
            var result = await _appointmentService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetAppointment)}/{{id}}")]
        public async Task<ActionResult<Appointment>> GetAppointment(Guid id)
        {
            var result = await _appointmentService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetAppointments)}/Filter")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            var appointment = new AppointmentModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    appointment = JsonConvert.DeserializeObject<AppointmentModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (appointment != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    appointment.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    appointment = JsonConvert.DeserializeObject<AppointmentModel>(body);
            }

            if (appointment == null)
                return BadRequest();

            var result = await _appointmentService.GetPaging(appointment);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportAppointment))]
        public async Task<ActionResult<Appointment>> ImportAppointment()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _appointmentService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportAppointment))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportAppointment()
        {
            var appointment = new AppointmentModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    appointment = JsonConvert.DeserializeObject<AppointmentModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (appointment != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    appointment.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    appointment = JsonConvert.DeserializeObject<AppointmentModel>(body);
            }

            if (appointment == null)
                return BadRequest();

            var result = await _appointmentService.Export(appointment);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
