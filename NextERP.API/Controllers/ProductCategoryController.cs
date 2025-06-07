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
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpPost(nameof(CreateOrEditProductCategory))]
        public async Task<ActionResult<ProductCategory>> CreateOrEditProductCategory([FromBody] ProductCategoryModel productCategory)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _productCategoryService.CreateOrEdit(productCategory.Id, productCategory);
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

        [HttpGet($"{nameof(GetProductCategory)}/{{id}}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(Guid id)
        {
            var result = await _productCategoryService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetProductCategories)}/Filter")]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories(Filter filter)
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
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportProductCategory(Filter filter)
        {
            var result = await _productCategoryService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
