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
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpPost(nameof(CreateOrEditBranch))]
        public async Task<ActionResult<Branch>> CreateOrEditBranch()
        {
            var branch = new BranchModel();

            if (Request.HasFormContentType)
            {
                var json = Request.Form["Json"];
                if (!string.IsNullOrEmpty(json))
                    branch = JsonConvert.DeserializeObject<BranchModel>(json!);

                //// Khi nào model có field file thì mở ra
                //if (branch != null)
                //{
                //    var files = Request.Form.Files.Where(s => s.Name == Constants.Files).ToList();
                //    branch.ImageFiles = files;
                //}
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(body))
                    branch = JsonConvert.DeserializeObject<BranchModel>(body);
            }

            if (branch == null)
                return BadRequest();

            var result = await _branchService.CreateOrEdit(branch);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete(nameof(DeleteBranch))]
        public async Task<ActionResult<APIBaseResult<bool>>> DeleteBranch(string ids)
        {
            var result = await _branchService.Delete(ids);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet($"{nameof(GetBranch)}/{{id}}")]
        public async Task<ActionResult<Branch>> GetBranch(Guid id)
        {
            var result = await _branchService.GetOne(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost($"{nameof(GetBranches)}/Filter")]
        public async Task<ActionResult<IEnumerable<Branch>>> GetBranches(Filter filter)
        {
            var result = await _branchService.GetPaging(filter);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(nameof(ImportBranch))]
        public async Task<ActionResult<Branch>> ImportBranch()
        {
            IFormFile excelFile = Request.Form.Files[Constants.ExcelFiles]!;

            if (excelFile != null)
            {
                var result = await _branchService.Import(excelFile);
                if (!result.IsSuccess)
                    return BadRequest(result);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost(nameof(ExportBranch))]
        public async Task<ActionResult<APIBaseResult<byte[]>>> ExportBranch(Filter filter)
        {
            var result = await _branchService.Export(filter);
            if (!result.IsSuccess || result == null || result.Result == null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
