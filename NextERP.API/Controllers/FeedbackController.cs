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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost(nameof(CreateOrEditFeedback))]
        public async Task<ActionResult<Feedback>> CreateOrEditFeedback([FromBody] FeedbackModel feedback)
        {
            // Sau này mở rộng cho phép truyền file xuống 
            //IFormFile excelFile = Request.Form.Files["Files"]!;

            var result = await _feedbackService.CreateOrEdit(feedback.Id, feedback);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteFeedback))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteFeedback(string ids)
        {
            var result = await _feedbackService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetFeedback)}/{{id}}")]
        public async Task<ActionResult<Feedback>> GetFeedback(Guid id)
        {
            var result = await _feedbackService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetFeedbacks)}/Filter")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks(Filter filter)
        {
            var result = await _feedbackService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportFeedback))]
        public async Task<ActionResult<Feedback>> ImportFeedback()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _feedbackService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportFeedback))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportFeedback(Filter filter)
        {
            var result = await _feedbackService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
