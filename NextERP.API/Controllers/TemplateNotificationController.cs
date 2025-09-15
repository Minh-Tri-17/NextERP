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
    public class TemplateNotificationController : ControllerBase
    {
        #region Infrastructure

        private readonly ITemplateNotificationService _templateNotificationService;

        public TemplateNotificationController(ITemplateNotificationService templateNotificationService)
        {
            _templateNotificationService = templateNotificationService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditTemplateNotification))]
        public async Task<ActionResult<TemplateNotification>> CreateOrEditTemplateNotification()
        {
            var templateNotification = new TemplateNotificationModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    templateNotification = JsonConvert.DeserializeObject<TemplateNotificationModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (templateNotification != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    templateNotification.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    templateNotification = JsonConvert.DeserializeObject<TemplateNotificationModel>(body);
            }

            if (templateNotification == null)
                return BadRequest();

            var result = await _templateNotificationService.CreateOrEdit(templateNotification);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteTemplateNotification))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteTemplateNotification(string ids)
        {
            var result = await _templateNotificationService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyTemplateNotification))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyTemplateNotification(string ids)
        {
            var result = await _templateNotificationService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetTemplateNotification)}/{{id}}")]
        public async Task<ActionResult<TemplateNotification>> GetTemplateNotification(Guid id)
        {
            var result = await _templateNotificationService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetTemplateNotifications)}/Filter")]
        public async Task<ActionResult<IEnumerable<TemplateNotification>>> GetTemplateNotifications(FilterModel filter)
        {
            var result = await _templateNotificationService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportTemplateNotification))]
        public async Task<ActionResult<TemplateNotification>> ImportTemplateNotification()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _templateNotificationService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportTemplateNotification))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportTemplateNotification(FilterModel filter)
        {
            var result = await _templateNotificationService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
