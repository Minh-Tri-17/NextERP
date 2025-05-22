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
    public class LeaveRequestsController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestsController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpPost(nameof(GetLeaveRequests))]
        public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetLeaveRequests(Filter filter)
        {
            var result = await _leaveRequestService.GetPaging(filter);
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

        [HttpPost(nameof(CreateOrEditLeaveRequest))]
        public async Task<ActionResult<LeaveRequest>> CreateOrEditLeaveRequest([FromBody] LeaveRequestModel leaveRequest)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _leaveRequestService.CreateOrEdit(leaveRequest.Id, leaveRequest);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteLeaveRequest))]
        public async Task<IActionResult> DeleteLeaveRequest(string ids)
        {
            var result = await _leaveRequestService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportLeaveRequest))]
        public async Task<ActionResult<LeaveRequest>> ImportLeaveRequest()
        {
            IFormFile excelFile = Request.Form.Files["ExcelFiles"]!;

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
        public async Task<IActionResult> ExportLeaveRequest(Filter filter)
        {
            var result = await _leaveRequestService.Export(filter);

            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            var fileName = string.Format(Constants.FileName, ObjectNames.LeaveRequest, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result, Constants.ContentType, fileName);
        }
    }
}
