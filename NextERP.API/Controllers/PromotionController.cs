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
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpPost(nameof(CreateOrEditPromotion))]
        public async Task<ActionResult<Promotion>> CreateOrEditPromotion()
        {
            var promotion = new PromotionModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    promotion = JsonConvert.DeserializeObject<PromotionModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (promotion != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    promotion.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    promotion = JsonConvert.DeserializeObject<PromotionModel>(body);
            }

            if (promotion == null)
                return BadRequest();

            var result = await _promotionService.CreateOrEdit(promotion);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePromotion))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePromotion(string ids)
        {
            var result = await _promotionService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyPromotion))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyPromotion(string ids)
        {
            var result = await _promotionService.DeletePermanently(ids);
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

        [HttpPost($"{nameof(GetPromotions)}/Filter")]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetPromotions(Filter filter)
        {
            var result = await _promotionService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportPromotion))]
        public async Task<ActionResult<Promotion>> ImportPromotion()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

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
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportPromotion(Filter filter)
        {
            var result = await _promotionService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
