using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class SpaServiceController : ControllerBase
    {
        private readonly ISpaServiceService _spaServiceService;

        public SpaServiceController(ISpaServiceService spaServiceService)
        {
            _spaServiceService = spaServiceService;
        }

        [HttpPost(nameof(CreateOrEditSpaService))]
        public async Task<ActionResult<SpaService>> CreateOrEditSpaService([FromBody] SpaServiceModel spaService)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _spaServiceService.CreateOrEdit(spaService.Id, spaService);
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

        [HttpGet($"{nameof(GetSpaService)}/{{id}}")]
        public async Task<ActionResult<SpaService>> GetSpaService(Guid id)
        {
            var result = await _spaServiceService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetSpaServices)}/Filter")]
        public async Task<ActionResult<IEnumerable<SpaService>>> GetSpaServices(Filter filter)
        {
            var result = await _spaServiceService.GetPaging(filter);
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
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportSpaService(Filter filter)
        {
            var result = await _spaServiceService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
