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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost(nameof(GetProducts))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(Filter filter)
        {
            var result = await _productService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetProduct)}/{{id}}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var result = await _productService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(CreateOrEditProduct))]
        public async Task<ActionResult<Product>> CreateOrEditProduct([FromBody] ProductModel product)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _productService.CreateOrEdit(product.Id, product);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteProduct))]
        public async Task<IActionResult> DeleteProduct(string ids)
        {
            var result = await _productService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportProduct))]
        public async Task<ActionResult<Product>> ImportProduct()
        {
            IFormFile excelFile = Request.Form.Files["ExcelFiles"]!;

            if (excelFile != null)
            {
                var result = await _productService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportProduct))]
        public async Task<IActionResult> ExportProduct(Filter filter)
        {
            var result = await _productService.Export(filter);

            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            var fileName = string.Format(Constants.FileName, ObjectNames.Product, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result, Constants.ContentType, fileName);
        }
    }
}
