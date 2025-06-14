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
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost(nameof(CreateOrEditInvoice))]
        public async Task<ActionResult<Invoice>> CreateOrEditInvoice()
        {
            var invoice = new InvoiceModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    invoice = JsonConvert.DeserializeObject<InvoiceModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (invoice != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    invoice.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    invoice = JsonConvert.DeserializeObject<InvoiceModel>(body);
            }

            if (invoice == null)
                return BadRequest();

            var result = await _invoiceService.CreateOrEdit(invoice);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteInvoice))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteInvoice(string ids)
        {
            var result = await _invoiceService.Delete(ids);
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

        [HttpPost($"{nameof(GetInvoices)}/Filter")]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(Filter filter)
        {
            var result = await _invoiceService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
