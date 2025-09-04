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
    public class SpaServiceController : ControllerBase
    {
        #region Infrastructure

        private readonly ISpaServiceService _spaServiceService;

        public SpaServiceController(ISpaServiceService spaServiceService)
        {
            _spaServiceService = spaServiceService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditSpaService))]
        public async Task<ActionResult<SpaService>> CreateOrEditSpaService()
        {
            var spaService = new SpaServiceModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    spaService = JsonConvert.DeserializeObject<SpaServiceModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (spaService != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    spaService.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    spaService = JsonConvert.DeserializeObject<SpaServiceModel>(body);
            }

            if (spaService == null)
                return BadRequest();

            var result = await _spaServiceService.CreateOrEdit(spaService);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteSpaService))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteSpaService(string ids)
        {
            var result = await _spaServiceService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlySpaService))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlySpaService(string ids)
        {
            var result = await _spaServiceService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetSpaService)}/{{id}}")]
        public async Task<ActionResult<SpaService>> GetSpaService(Guid id)
        {
            var result = await _spaServiceService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetSpaServices)}/Filter")]
        public async Task<ActionResult<IEnumerable<SpaService>>> GetSpaServices()
        {
            var spaService = new SpaServiceModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    spaService = JsonConvert.DeserializeObject<SpaServiceModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (spaService != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    spaService.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    spaService = JsonConvert.DeserializeObject<SpaServiceModel>(body);
            }

            if (spaService == null)
                return BadRequest();

            var result = await _spaServiceService.GetPaging(spaService);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportSpaService))]
        public async Task<ActionResult<SpaService>> ImportSpaService()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _spaServiceService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportSpaService))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportSpaService()
        {
            var spaService = new SpaServiceModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    spaService = JsonConvert.DeserializeObject<SpaServiceModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (spaService != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    spaService.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    spaService = JsonConvert.DeserializeObject<SpaServiceModel>(body);
            }

            if (spaService == null)
                return BadRequest();

            var result = await _spaServiceService.Export(spaService);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
