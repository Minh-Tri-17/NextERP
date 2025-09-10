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
    public class NotificationController : ControllerBase
    {
        #region Infrastructure

        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditNotification))]
        public async Task<ActionResult<Notification>> CreateOrEditNotification()
        {
            var notification = new NotificationModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    notification = JsonConvert.DeserializeObject<NotificationModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (notification != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    notification.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    notification = JsonConvert.DeserializeObject<NotificationModel>(body);
            }

            if (notification == null)
                return BadRequest();

            var result = await _notificationService.CreateOrEdit(notification);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteNotification))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteNotification(string ids)
        {
            var result = await _notificationService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyNotification))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyNotification(string ids)
        {
            var result = await _notificationService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetNotification)}/{{id}}")]
        public async Task<ActionResult<Notification>> GetNotification(Guid id)
        {
            var result = await _notificationService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetNotifications)}/Filter")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications(Filter filter)
        {
            var result = await _notificationService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportNotification))]
        public async Task<ActionResult<Notification>> ImportNotification()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _notificationService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportNotification))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportNotification(Filter filter)
        {
            var result = await _notificationService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
