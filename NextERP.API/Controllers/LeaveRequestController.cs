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
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpPost(nameof(CreateOrEditLeaveRequest))]
        public async Task<ActionResult<LeaveRequest>> CreateOrEditLeaveRequest()
        {
            var leaveRequest = new LeaveRequestModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    leaveRequest = JsonConvert.DeserializeObject<LeaveRequestModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (leaveRequest != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    leaveRequest.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    leaveRequest = JsonConvert.DeserializeObject<LeaveRequestModel>(body);
            }

            if (leaveRequest == null)
                return BadRequest();

            var result = await _leaveRequestService.CreateOrEdit(leaveRequest);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteLeaveRequest))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteLeaveRequest(string ids)
        {
            var result = await _leaveRequestService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetLeaveRequest)}/{{id}}")]
        public async Task<ActionResult<LeaveRequest>> GetLeaveRequest(Guid id)
        {
            var result = await _leaveRequestService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetLeaveRequests)}/Filter")]
        public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetLeaveRequests(Filter filter)
        {
            var result = await _leaveRequestService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportLeaveRequest))]
        public async Task<ActionResult<LeaveRequest>> ImportLeaveRequest()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _leaveRequestService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportLeaveRequest))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportLeaveRequest(Filter filter)
        {
            var result = await _leaveRequestService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
