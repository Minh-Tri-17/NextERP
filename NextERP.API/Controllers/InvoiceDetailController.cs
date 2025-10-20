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
    public class InvoiceDetailController : ControllerBase
    {
        #region Infrastructure

        private readonly IInvoiceDetailService _invoiceDetailService;

        public InvoiceDetailController(IInvoiceDetailService invoiceDetailService)
        {
            _invoiceDetailService = invoiceDetailService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditInvoiceDetail))]
        public async Task<ActionResult<InvoiceDetail>> CreateOrEditInvoiceDetail()
        {
            var invoiceDetail = new InvoiceDetailModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    invoiceDetail = JsonConvert.DeserializeObject<InvoiceDetailModel>(json!);

            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    invoiceDetail = JsonConvert.DeserializeObject<InvoiceDetailModel>(body);
            }

            if (invoiceDetail == null)
                return BadRequest();

            var result = await _invoiceDetailService.CreateOrEdit(invoiceDetail);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteInvoiceDetail))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteInvoiceDetail(string ids)
        {
            var result = await _invoiceDetailService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetInvoiceDetails)}/Filter")]
        public async Task<ActionResult<IEnumerable<InvoiceDetail>>> GetInvoiceDetails(FilterModel filter)
        {
            var result = await _invoiceDetailService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
