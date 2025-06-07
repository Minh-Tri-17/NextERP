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
    public class SupplierOrderDetailController : ControllerBase
    {
        private readonly ISupplierOrderDetailService _supplierOrderDetailService;

        public SupplierOrderDetailController(ISupplierOrderDetailService supplierOrderDetailService)
        {
            _supplierOrderDetailService = supplierOrderDetailService;
        }

        [HttpPost(nameof(CreateOrEditSupplierOrderDetail))]
        public async Task<ActionResult<SupplierOrderDetail>> CreateOrEditSupplierOrderDetail([FromBody] SupplierOrderDetailModel supplierOrderDetail)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _supplierOrderDetailService.CreateOrEdit(supplierOrderDetail.Id, supplierOrderDetail);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteSupplierOrderDetail))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteSupplierOrderDetail(string ids)
        {
            var result = await _supplierOrderDetailService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetSupplierOrderDetail)}/{{id}}")]
        public async Task<ActionResult<SupplierOrderDetail>> GetSupplierOrderDetail(Guid id)
        {
            var result = await _supplierOrderDetailService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetSupplierOrderDetails)}/Filter")]
        public async Task<ActionResult<IEnumerable<SupplierOrderDetail>>> GetSupplierOrderDetails(Filter filter)
        {
            var result = await _supplierOrderDetailService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
