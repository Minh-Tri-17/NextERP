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
        private readonly ISupplierOrderService _supplierOrderService;

        public SupplierOrderController(ISupplierOrderService supplierOrderService)
        {
            _supplierOrderService = supplierOrderService;
        }

        [HttpPost(nameof(CreateOrEditSupplierOrder))]
        public async Task<ActionResult<SupplierOrder>> CreateOrEditSupplierOrder()
        {
            var supplierOrder = new SupplierOrderModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    supplierOrder = JsonConvert.DeserializeObject<SupplierOrderModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (supplierOrder != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    supplierOrder.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    supplierOrder = JsonConvert.DeserializeObject<SupplierOrderModel>(body);
            }

            if (supplierOrder == null)
                return BadRequest();

            var result = await _supplierOrderService.CreateOrEdit(supplierOrder);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

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
        public async Task<ActionResult<IEnumerable<SupplierOrder>>> GetSupplierOrders(Filter filter)
        {
            var result = await _supplierOrderService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
