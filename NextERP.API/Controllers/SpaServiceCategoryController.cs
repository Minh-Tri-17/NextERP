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
    public class SpaServiceCategoryController : ControllerBase
    {
        private readonly ISpaServiceCategoryService _spaServiceCategoryService;

        public SpaServiceCategoryController(ISpaServiceCategoryService spaServiceCategoryService)
        {
            _spaServiceCategoryService = spaServiceCategoryService;
        }

        [HttpPost(nameof(CreateOrEditSpaServiceCategory))]
        public async Task<ActionResult<SpaServiceCategory>> CreateOrEditSpaServiceCategory()
        {
            var spaServiceCategory = new SpaServiceCategoryModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    spaServiceCategory = JsonConvert.DeserializeObject<SpaServiceCategoryModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (spaServiceCategory != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    spaServiceCategory.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    spaServiceCategory = JsonConvert.DeserializeObject<SpaServiceCategoryModel>(body);
            }

            if (spaServiceCategory == null)
                return BadRequest();

            var result = await _spaServiceCategoryService.CreateOrEdit(spaServiceCategory);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteSpaServiceCategory))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteSpaServiceCategory(string ids)
        {
            var result = await _spaServiceCategoryService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlySpaServiceCategory))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlySpaServiceCategory(string ids)
        {
            var result = await _spaServiceCategoryService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetSpaServiceCategory)}/{{id}}")]
        public async Task<ActionResult<SpaServiceCategory>> GetSpaServiceCategory(Guid id)
        {
            var result = await _spaServiceCategoryService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetSpaServiceCategories)}/Filter")]
        public async Task<ActionResult<IEnumerable<SpaServiceCategory>>> GetSpaServiceCategories(Filter filter)
        {
            var result = await _spaServiceCategoryService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportSpaServiceCategory))]
        public async Task<ActionResult<SpaServiceCategory>> ImportSpaServiceCategory()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _spaServiceCategoryService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportSpaServiceCategory))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportSpaServiceCategory(Filter filter)
        {
            var result = await _spaServiceCategoryService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
