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
    public class CustomerController : ControllerBase
    {
        #region Infrastructure

        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #endregion

        #region Default Operations

        [HttpPost(nameof(CreateOrEditCustomer))]
        public async Task<ActionResult<Customer>> CreateOrEditCustomer()
        {
            var customer = new CustomerModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    customer = JsonConvert.DeserializeObject<CustomerModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (customer != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    customer.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    customer = JsonConvert.DeserializeObject<CustomerModel>(body);
            }

            if (customer == null)
                return BadRequest();

            var result = await _customerService.CreateOrEdit(customer);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteCustomer))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteCustomer(string ids)
        {
            var result = await _customerService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyCustomer))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyCustomer(string ids)
        {
            var result = await _customerService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetCustomer)}/{{id}}")]
        public async Task<ActionResult<Customer>> GetCustomer(Guid id)
        {
            var result = await _customerService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetCustomers)}/Filter")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customer = new CustomerModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    customer = JsonConvert.DeserializeObject<CustomerModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (customer != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    customer.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    customer = JsonConvert.DeserializeObject<CustomerModel>(body);
            }

            if (customer == null)
                return BadRequest();

            var result = await _customerService.GetPaging(customer);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportCustomer))]
        public async Task<ActionResult<Customer>> ImportCustomer()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _customerService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportCustomer))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportCustomer()
        {
            var customer = new CustomerModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    customer = JsonConvert.DeserializeObject<CustomerModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (customer != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    customer.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    customer = JsonConvert.DeserializeObject<CustomerModel>(body);
            }

            if (customer == null)
                return BadRequest();

            var result = await _customerService.Export(customer);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
