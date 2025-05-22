using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
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
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpPost(nameof(GetPromotions))]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetPromotions(Filter filter)
        {
            var result = await _promotionService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetPromotion)}/{{id}}")]
        public async Task<ActionResult<Promotion>> GetPromotion(Guid id)
        {
            var result = await _promotionService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(CreateOrEditPromotion))]
        public async Task<ActionResult<Promotion>> CreateOrEditPromotion([FromBody] PromotionModel promotion)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _promotionService.CreateOrEdit(promotion.Id, promotion);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePromotion))]
        public async Task<IActionResult> DeletePromotion(string ids)
        {
            var result = await _promotionService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportPromotion))]
        public async Task<ActionResult<Promotion>> ImportPromotion()
        {
            IFormFile excelFile = Request.Form.Files["ExcelFiles"]!;

            if (excelFile != null)
            {
                var result = await _promotionService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportPromotion))]
        public async Task<IActionResult> ExportPromotion(Filter filter)
        {
            var result = await _promotionService.Export(filter);

            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            var fileName = string.Format(Constants.FileName, ObjectNames.Promotion, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result, Constants.ContentType, fileName);
        }
    }
}
