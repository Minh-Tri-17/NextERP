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
    public class TrainingSessionController : ControllerBase
    {
        private readonly ITrainingSessionService _trainingSessionService;

        public TrainingSessionController(ITrainingSessionService trainingSessionService)
        {
            _trainingSessionService = trainingSessionService;
        }

        [HttpPost(nameof(CreateOrEditTrainingSession))]
        public async Task<ActionResult<TrainingSession>> CreateOrEditTrainingSession()
        {
            var trainingSession = new TrainingSessionModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    trainingSession = JsonConvert.DeserializeObject<TrainingSessionModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (trainingSession != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    trainingSession.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    trainingSession = JsonConvert.DeserializeObject<TrainingSessionModel>(body);
            }

            if (trainingSession == null)
                return BadRequest();

            var result = await _trainingSessionService.CreateOrEdit(trainingSession);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteTrainingSession))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteTrainingSession(string ids)
        {
            var result = await _trainingSessionService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeletePermanentlyTrainingSession))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeletePermanentlyTrainingSession(string ids)
        {
            var result = await _trainingSessionService.DeletePermanently(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetTrainingSession)}/{{id}}")]
        public async Task<ActionResult<TrainingSession>> GetTrainingSession(Guid id)
        {
            var result = await _trainingSessionService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetTrainingSessions)}/Filter")]
        public async Task<ActionResult<IEnumerable<TrainingSession>>> GetTrainingSessions(Filter filter)
        {
            var result = await _trainingSessionService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportTrainingSession))]
        public async Task<ActionResult<TrainingSession>> ImportTrainingSession()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _trainingSessionService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportTrainingSession))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportTrainingSession(Filter filter)
        {
            var result = await _trainingSessionService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
