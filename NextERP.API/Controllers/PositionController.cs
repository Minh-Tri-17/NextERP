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
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpPost(nameof(CreateOrEditPosition))]
        public async Task<ActionResult<Position>> CreateOrEditPosition()
        {
            var position = new PositionModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    position = JsonConvert.DeserializeObject<PositionModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (position != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    position.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    position = JsonConvert.DeserializeObject<PositionModel>(body);
            }

            if (position == null)
                return BadRequest();

            var result = await _positionService.CreateOrEdit(position);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePosition))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePosition(string ids)
        {
            var result = await _positionService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyPosition))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyPosition(string ids)
        {
            var result = await _positionService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetPosition)}/{{id}}")]
        public async Task<ActionResult<Position>> GetPosition(Guid id)
        {
            var result = await _positionService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetPositions)}/Filter")]
        public async Task<ActionResult<IEnumerable<Position>>> GetPositions(Filter filter)
        {
            var result = await _positionService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportPosition))]
        public async Task<ActionResult<Position>> ImportPosition()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _positionService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportPosition))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportPosition(Filter filter)
        {
            var result = await _positionService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
