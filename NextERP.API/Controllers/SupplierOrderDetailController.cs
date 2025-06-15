using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public async Task<ActionResult<SupplierOrderDetail>> CreateOrEditSupplierOrderDetail()
        {
            var supplierOrderDetail = new SupplierOrderDetailModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    supplierOrderDetail = JsonConvert.DeserializeObject<SupplierOrderDetailModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (supplierOrderDetail != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    supplierOrderDetail.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    supplierOrderDetail = JsonConvert.DeserializeObject<SupplierOrderDetailModel>(body);
            }

            if (supplierOrderDetail == null)
                return BadRequest();

            var result = await _supplierOrderDetailService.CreateOrEdit(supplierOrderDetail);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteSupplierOrderDetail))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteSupplierOrderDetail(string ids)
        {
            var result = await _supplierOrderDetailService.DeletePermanently(ids);
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
