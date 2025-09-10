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
    public class SupplierController : ControllerBase
    {
        #region Infrastructure

        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditSupplier))]
        public async Task<ActionResult<Supplier>> CreateOrEditSupplier()
        {
            var supplier = new SupplierModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    supplier = JsonConvert.DeserializeObject<SupplierModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (supplier != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    supplier.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    supplier = JsonConvert.DeserializeObject<SupplierModel>(body);
            }

            if (supplier == null)
                return BadRequest();

            var result = await _supplierService.CreateOrEdit(supplier);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteSupplier))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteSupplier(string ids)
        {
            var result = await _supplierService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlySupplier))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlySupplier(string ids)
        {
            var result = await _supplierService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetSupplier)}/{{id}}")]
        public async Task<ActionResult<Supplier>> GetSupplier(Guid id)
        {
            var result = await _supplierService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetSuppliers)}/Filter")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers(Filter filter)
        {
            var result = await _supplierService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportSupplier))]
        public async Task<ActionResult<Supplier>> ImportSupplier()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _supplierService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportSupplier))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportSupplier(Filter filter)
        {
            var result = await _supplierService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
