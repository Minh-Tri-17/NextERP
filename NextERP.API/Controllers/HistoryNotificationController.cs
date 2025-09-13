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
    public class HistoryNotificationController : ControllerBase
    {
        #region Infrastructure

        private readonly IHistoryNotificationService _historyNotificationService;

        public HistoryNotificationController(IHistoryNotificationService historyNotificationService)
        {
            _historyNotificationService = historyNotificationService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditHistoryNotification))]
        public async Task<ActionResult<HistoryNotification>> CreateOrEditHistoryNotification()
        {
            var historyNotification = new HistoryNotificationModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    historyNotification = JsonConvert.DeserializeObject<HistoryNotificationModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (historyNotification != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    historyNotification.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    historyNotification = JsonConvert.DeserializeObject<HistoryNotificationModel>(body);
            }

            if (historyNotification == null)
                return BadRequest();

            var result = await _historyNotificationService.CreateOrEdit(historyNotification);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteHistoryNotification))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteHistoryNotification(string ids)
        {
            var result = await _historyNotificationService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyHistoryNotification))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyHistoryNotification(string ids)
        {
            var result = await _historyNotificationService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetHistoryNotification)}/{{id}}")]
        public async Task<ActionResult<HistoryNotification>> GetHistoryNotification(Guid id)
        {
            var result = await _historyNotificationService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetHistoryNotifications)}/Filter")]
        public async Task<ActionResult<IEnumerable<HistoryNotification>>> GetHistoryNotifications(Filter filter)
        {
            var result = await _historyNotificationService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportHistoryNotification))]
        public async Task<ActionResult<HistoryNotification>> ImportHistoryNotification()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _historyNotificationService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportHistoryNotification))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportHistoryNotification(Filter filter)
        {
            var result = await _historyNotificationService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
