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
    public class SupplierOrdersController : ControllerBase
    {
        private readonly ISupplierOrderService _supplierOrderService;

        public SupplierOrdersController(ISupplierOrderService supplierOrderService)
        {
            _supplierOrderService = supplierOrderService;
        }

        [HttpPost(nameof(GetSupplierOrders))]
        public async Task<ActionResult<IEnumerable<SupplierOrder>>> GetSupplierOrders(Filter filter)
        {
            var result = await _supplierOrderService.GetPaging(filter);
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

        [HttpPost(nameof(CreateOrEditSupplierOrder))]
        public async Task<ActionResult<SupplierOrder>> CreateOrEditSupplierOrder([FromBody] SupplierOrderModel supplierOrder)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _supplierOrderService.CreateOrEdit(supplierOrder.Id, supplierOrder);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteSupplierOrder))]
        public async Task<IActionResult> DeleteSupplierOrder(string ids)
        {
            var result = await _supplierOrderService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
