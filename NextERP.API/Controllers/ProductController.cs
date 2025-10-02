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
    public class ProductController : ControllerBase
    {
        #region Infrastructure

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditProduct))]
        public async Task<ActionResult<Product>> CreateOrEditProduct()
        {
            var product = new ProductModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    product = JsonConvert.DeserializeObject<ProductModel>(json!);

                if (product != null)
                {
                    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                    product.ImageFiles = files;
                }
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    product = JsonConvert.DeserializeObject<ProductModel>(body);
            }

            if (product == null)
                return BadRequest();

            var result = await _productService.CreateOrEdit(product);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteProduct))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteProduct(string ids)
        {
            var result = await _productService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyProduct))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyProduct(string ids)
        {
            var result = await _productService.DeletePermanently(ids);
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

        [HttpPost($"{nameof(GetProducts)}/Filter")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(FilterModel filter)
        {
            var result = await _productService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportProduct))]
        public async Task<ActionResult<Product>> ImportProduct()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

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
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportProduct(FilterModel filter)
        {
            var result = await _productService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        [HttpGet($"{nameof(GetImageProduct)}/{{productId}}/Image/{{imagePath}}")]
        public async Task<IActionResult> GetImageProduct(Guid productId, string imagePath)
        {
            byte[] imageData = await _productService.GetImageBytes(productId, imagePath);

            return File(imageData, "image/jpg");
        }

        #endregion
    }
}
