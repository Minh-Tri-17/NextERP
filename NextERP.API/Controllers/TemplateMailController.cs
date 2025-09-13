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
    public class TemplateMailController : ControllerBase
    {
        #region Infrastructure

        private readonly ITemplateMailService _templateMailService;

        public TemplateMailController(ITemplateMailService templateMailService)
        {
            _templateMailService = templateMailService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditTemplateMail))]
        public async Task<ActionResult<TemplateMail>> CreateOrEditTemplateMail()
        {
            var templateMail = new TemplateMailModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    templateMail = JsonConvert.DeserializeObject<TemplateMailModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (templateMail != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    templateMail.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    templateMail = JsonConvert.DeserializeObject<TemplateMailModel>(body);
            }

            if (templateMail == null)
                return BadRequest();

            var result = await _templateMailService.CreateOrEdit(templateMail);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteTemplateMail))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteTemplateMail(string ids)
        {
            var result = await _templateMailService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyTemplateMail))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyTemplateMail(string ids)
        {
            var result = await _templateMailService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetTemplateMail)}/{{id}}")]
        public async Task<ActionResult<TemplateMail>> GetTemplateMail(Guid id)
        {
            var result = await _templateMailService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetTemplateMails)}/Filter")]
        public async Task<ActionResult<IEnumerable<TemplateMail>>> GetTemplateMails(Filter filter)
        {
            var result = await _templateMailService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportTemplateMail))]
        public async Task<ActionResult<TemplateMail>> ImportTemplateMail()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _templateMailService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportTemplateMail))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportTemplateMail(Filter filter)
        {
            var result = await _templateMailService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        [HttpPost(nameof(SendMail))]
        public async Task<ActionResult<APIBaseResult<bool>>> SendMail()
        {
            var result = await _templateMailService.SendMail();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion
    }
}
