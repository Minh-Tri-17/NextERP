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
    public class ProductCategoryController : ControllerBase
    {
        #region Infrastructure

        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditProductCategory))]
        public async Task<ActionResult<ProductCategory>> CreateOrEditProductCategory()
        {
            var productCategory = new ProductCategoryModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    productCategory = JsonConvert.DeserializeObject<ProductCategoryModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (productCategory != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    productCategory.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    productCategory = JsonConvert.DeserializeObject<ProductCategoryModel>(body);
            }

            if (productCategory == null)
                return BadRequest();

            var result = await _productCategoryService.CreateOrEdit(productCategory);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteProductCategory))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteProductCategory(string ids)
        {
            var result = await _productCategoryService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyProductCategory))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyProductCategory(string ids)
        {
            var result = await _productCategoryService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetProductCategory)}/{{id}}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(Guid id)
        {
            var result = await _productCategoryService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetProductCategories)}/Filter")]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories(FilterModel filter)
        {
            var result = await _productCategoryService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportProductCategory))]
        public async Task<ActionResult<ProductCategory>> ImportProductCategory()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _productCategoryService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportProductCategory))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportProductCategory(FilterModel filter)
        {
            var result = await _productCategoryService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
