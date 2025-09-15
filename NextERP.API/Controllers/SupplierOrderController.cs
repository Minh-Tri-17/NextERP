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
    public class SupplierOrderController : ControllerBase
    {
        #region Infrastructure

        private readonly ISupplierOrderService _supplierOrderService;

        public SupplierOrderController(ISupplierOrderService supplierOrderService)
        {
            _supplierOrderService = supplierOrderService;
        }

        #endregion

        #region Default Operations

        [HttpDelete(nameof(DeleteSupplierOrder))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteSupplierOrder(string ids)
        {
            var result = await _supplierOrderService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlySupplierOrder))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlySupplierOrder(string ids)
        {
            var result = await _supplierOrderService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetSupplierOrder)}/{{id}}")]
        public async Task<ActionResult<SupplierOrder>> GetSupplierOrder(Guid id)
        {
            var result = await _supplierOrderService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetSupplierOrders)}/Filter")]
        public async Task<ActionResult<IEnumerable<SupplierOrder>>> GetSupplierOrders(FilterModel filter)
        {
            var result = await _supplierOrderService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportSupplierOrder))]
        public async Task<ActionResult<Supplier>> ImportSupplierOrder()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _supplierOrderService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportSupplierOrder))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportSupplierOrder(FilterModel filter)
        {
            var result = await _supplierOrderService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
