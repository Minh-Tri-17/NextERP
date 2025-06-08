using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public FeedbackService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(FeedbackModel request)
        {
            if (request.Id == Guid.Empty)
            {
                var feedback = new Feedback();
                DataHelper.MapAudit(request, feedback, _currentUser.UserName);

                await _context.Feedbacks.AddAsync(feedback);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var feedback = await _context.Feedbacks.FindAsync(request.Id);
                if (feedback == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, feedback, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listFeedbackId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listFeedback = await _context.Feedbacks
                .Where(x => listFeedbackId.Contains(x.Id))
                .ToListAsync();

            foreach (var feedback in listFeedback)
            {
                feedback.IsDelete = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<FeedbackModel>> GetOne(Guid id)
        {
            var feedback = await _context.Feedbacks
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(x => x.Id == id);

            if (feedback == null)
                return new APIErrorResult<FeedbackModel>(Messages.NotFoundGet);

            var feedbackModel = DataHelper.Mapping<Feedback, FeedbackModel>(feedback);

            return new APISuccessResult<FeedbackModel>(Messages.GetResultSuccess, feedbackModel);
        }

        public async Task<APIBaseResult<PagingResult<FeedbackModel>>> GetPaging(Filter filter)
        {
            IQueryable<Feedback> query = _context.Feedbacks
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.FeedbackCode)
                    && x.FeedbackCode.ToLower().Contains(keyword));
            }

            var totalCount = await query.CountAsync();

            if (filter.AllowPaging)
            {
                query = query.Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize);
            }

            if (!string.IsNullOrEmpty(filter.Ids))
            {
                List<Guid> listFeedbackId = filter.Ids.Split(',')
                     .Select(id => DataHelper.GetGuid(id.Trim()))
                     .Where(guid => guid != Guid.Empty)
                     .ToList();

                query = query.Where(x => listFeedbackId.Contains(x.Id));
            }

            var listFeedback = await query
                .OrderByDescending(x => x.DateFeedback)
                .ToListAsync();

            if (!listFeedback.Any())
                return new APIErrorResult<PagingResult<FeedbackModel>>(Messages.NotFoundGetList);

            var listFeedbackModel = DataHelper.MappingList<Feedback, FeedbackModel>(listFeedback);
            var pageResult = new PagingResult<FeedbackModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listFeedbackModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listFeedbackModel
            };

            return new APISuccessResult<PagingResult<FeedbackModel>>(Messages.GetListResultSuccess, pageResult);
        }

        public async Task<APIBaseResult<bool>> Import(IFormFile fileImport)
        {
            var stream = new MemoryStream();
            await fileImport.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);

            // Header data
            var headerRow = sheet.GetRow(0);

            var listFeedbackModel = new List<FeedbackModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);

                if (row != null)
                {
                    FeedbackModel feedbackModel = DataHelper.CopyImport<FeedbackModel>(headerRow, row);
                    listFeedbackModel.Add(feedbackModel);
                }
            }

            var listFeedback = new List<Feedback>();
            DataHelper.MapListAudit<FeedbackModel, Feedback>(listFeedbackModel, listFeedback, _currentUser.UserName);

            await _context.Feedbacks.AddRangeAsync(listFeedback);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(Filter filter)
        {
            var data = await GetPaging(filter);
            var items = data?.Result?.Items;

            if (items == null || items.Count == 0)
                return new APIErrorResult<byte[]>(Messages.NotFoundGetList);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.Feedback);

            var listFeedback = DataHelper.MappingList<FeedbackModel, Feedback>(items);
            DataHelper.CopyExport(worksheet, listFeedback);

            var stream = new MemoryStream();
            workbook.SaveAs(stream); // Ghi nội dung của workbook(Excel) vào stream
            var bytes = stream.ToArray(); // Chuyển toàn bộ nội dung stream thành mảng byte
            if (bytes.Length > 0)
                return new APISuccessResult<byte[]>(Messages.ExportSuccess, bytes);

            return new APIErrorResult<byte[]>(Messages.ExportFailed);
        }
    }
}
