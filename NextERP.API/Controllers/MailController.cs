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
    public class MailController : ControllerBase
    {
        #region Infrastructure

        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditMail))]
        public async Task<ActionResult<Mail>> CreateOrEditMail()
        {
            var mail = new MailModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    mail = JsonConvert.DeserializeObject<MailModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (mail != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    mail.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    mail = JsonConvert.DeserializeObject<MailModel>(body);
            }

            if (mail == null)
                return BadRequest();

            var result = await _mailService.CreateOrEdit(mail);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteMail))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteMail(string ids)
        {
            var result = await _mailService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyMail))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyMail(string ids)
        {
            var result = await _mailService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetMail)}/{{id}}")]
        public async Task<ActionResult<Mail>> GetMail(Guid id)
        {
            var result = await _mailService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetMails)}/Filter")]
        public async Task<ActionResult<IEnumerable<Mail>>> GetMails(Filter filter)
        {
            var result = await _mailService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportMail))]
        public async Task<ActionResult<Mail>> ImportMail()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _mailService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportMail))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportMail(Filter filter)
        {
            var result = await _mailService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
