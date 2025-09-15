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
    public class HistoryMailController : ControllerBase
    {
        #region Infrastructure

        private readonly IHistoryMailService _historyMailService;

        public HistoryMailController(IHistoryMailService historyMailService)
        {
            _historyMailService = historyMailService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditHistoryMail))]
        public async Task<ActionResult<HistoryMail>> CreateOrEditHistoryMail()
        {
            var historyMail = new HistoryMailModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    historyMail = JsonConvert.DeserializeObject<HistoryMailModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (historyMail != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    historyMail.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    historyMail = JsonConvert.DeserializeObject<HistoryMailModel>(body);
            }

            if (historyMail == null)
                return BadRequest();

            var result = await _historyMailService.CreateOrEdit(historyMail);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteHistoryMail))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteHistoryMail(string ids)
        {
            var result = await _historyMailService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyHistoryMail))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyHistoryMail(string ids)
        {
            var result = await _historyMailService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetHistoryMail)}/{{id}}")]
        public async Task<ActionResult<HistoryMail>> GetHistoryMail(Guid id)
        {
            var result = await _historyMailService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetHistoryMails)}/Filter")]
        public async Task<ActionResult<IEnumerable<HistoryMail>>> GetHistoryMails(FilterModel filter)
        {
            var result = await _historyMailService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportHistoryMail))]
        public async Task<ActionResult<HistoryMail>> ImportHistoryMail()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _historyMailService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportHistoryMail))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportHistoryMail(FilterModel filter)
        {
            var result = await _historyMailService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
