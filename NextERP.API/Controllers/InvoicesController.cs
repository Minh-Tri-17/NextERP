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
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost(nameof(GetInvoices))]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(Filter filter)
        {
            var result = await _invoiceService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetInvoice)}/{{id}}")]
        public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
        {
            var result = await _invoiceService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(CreateOrEditInvoice))]
        public async Task<ActionResult<Invoice>> CreateOrEditInvoice([FromBody] InvoiceModel invoice)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _invoiceService.CreateOrEdit(invoice.Id, invoice);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteInvoice))]
        public async Task<IActionResult> DeleteInvoice(string ids)
        {
            var result = await _invoiceService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
