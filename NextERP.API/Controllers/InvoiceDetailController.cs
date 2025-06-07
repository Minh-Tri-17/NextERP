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
    public class InvoiceDetailController : ControllerBase
    {
        private readonly IInvoiceDetailService _invoiceDetailService;

        public InvoiceDetailController(IInvoiceDetailService invoiceDetailService)
        {
            _invoiceDetailService = invoiceDetailService;
        }

        [HttpPost(nameof(CreateOrEditInvoiceDetail))]
        public async Task<ActionResult<InvoiceDetail>> CreateOrEditInvoiceDetail([FromBody] InvoiceDetailModel invoiceDetail)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _invoiceDetailService.CreateOrEdit(invoiceDetail.Id, invoiceDetail);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteInvoiceDetail))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteInvoiceDetail(string ids)
        {
            var result = await _invoiceDetailService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetInvoiceDetail)}/{{id}}")]
        public async Task<ActionResult<InvoiceDetail>> GetInvoiceDetail(Guid id)
        {
            var result = await _invoiceDetailService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetInvoiceDetails)}/Filter")]
        public async Task<ActionResult<IEnumerable<InvoiceDetail>>> GetInvoiceDetails(Filter filter)
        {
            var result = await _invoiceDetailService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
