using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.Util;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Service
{
    public class TemplateMailService : ITemplateMailService
    {
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public TemplateMailService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(TemplateMailModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

            if (id == Guid.Empty)
            {
                var templateMail = new TemplateMail();
                DataHelper.MapAudit(request, templateMail, _currentUser.UserName);

                await _context.TemplateMails.AddAsync(templateMail);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var templateMail = await _context.TemplateMails.FindAsync(id);
                if (templateMail == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, templateMail, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listTemplateMailId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listTemplateMail = await _context.TemplateMails
                .Where(s => listTemplateMailId.Contains(s.Id))
                .ToListAsync();

            foreach (var templateMail in listTemplateMail)
            {
                templateMail.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listTemplateMailId = ids.Split(',')
                 .Select(id => DataHelper.GetGuid(id.Trim()))
                 .Where(guid => guid != Guid.Empty)
                 .ToList();

            var listTemplateMail = await _context.TemplateMails
                .Where(s => listTemplateMailId.Contains(s.Id))
                .ToListAsync();

            foreach (var templateMail in listTemplateMail)
            {
                _context.TemplateMails.Remove(templateMail); // Xóa vĩnh viễn
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<TemplateMailModel>> GetOne(Guid id)
        {
            var templateMail = await _context.TemplateMails
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(s => s.Id == id);

            if (templateMail == null)
                return new APIErrorResult<TemplateMailModel>(Messages.NotFoundGet);

            var templateMailModel = DataHelper.Mapping<TemplateMail, TemplateMailModel>(templateMail);

            return new APISuccessResult<TemplateMailModel>(Messages.GetResultSuccess, templateMailModel);
        }

        public async Task<APIBaseResult<PagingResult<TemplateMailModel>>> GetPaging(FilterModel filter)
        {
            IQueryable<TemplateMail> query = _context.TemplateMails.AsNoTracking(); // Không theo dõi thay đổi của thực thể

            query = query.ApplyCommonFilters(filter);

            var totalCount = await query.CountAsync();

            query = query.ApplyPaging(filter);

            var listTemplateMail = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listTemplateMailModel = DataHelper.MappingList<TemplateMail, TemplateMailModel>(listTemplateMail);
            var pageResult = new PagingResult<TemplateMailModel>()
            {
                TotalRecord = totalCount,
                PageRecord = listTemplateMailModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listTemplateMailModel
            };

            return new APISuccessResult<PagingResult<TemplateMailModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listTemplateMailModel = new List<TemplateMailModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);
                if (row == null || row.Cells.All(c => c.CellType == NPOI.SS.UserModel.CellType.Blank))
                    continue; // Bỏ qua hàng trống

                TemplateMailModel templateMailModel = DataHelper.CopyImport<TemplateMailModel>(headerRow, row);
                listTemplateMailModel.Add(templateMailModel);
            }

            var listTemplateMail = new List<TemplateMail>();
            DataHelper.MapListAudit<TemplateMailModel, TemplateMail>(listTemplateMailModel, listTemplateMail, _currentUser.UserName);

            await _context.TemplateMails.AddRangeAsync(listTemplateMail);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.ImportSuccess, true);

            return new APIErrorResult<bool>(Messages.ImportFailed);
        }

        public async Task<APIBaseResult<byte[]>> Export(FilterModel filter)
        {
            var data = await GetPaging(filter);
            var items = data?.Result?.Items ?? new List<TemplateMailModel>();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(TableName.TemplateMail);

            var listTemplateMail = DataHelper.MappingList<TemplateMailModel, TemplateMail>(items);
            DataHelper.CopyExport(worksheet, listTemplateMail);

            var stream = new MemoryStream();
            workbook.SaveAs(stream); // Ghi nội dung của workbook(Excel) vào stream
            var bytes = stream.ToArray(); // Chuyển toàn bộ nội dung stream thành mảng byte
            if (bytes.Length > 0)
                return new APISuccessResult<byte[]>(Messages.ExportSuccess, bytes);

            return new APIErrorResult<byte[]>(Messages.ExportFailed);
        }

        #endregion

        #region Custom Operations

        public async Task<APIBaseResult<bool>> SendMail(MailModel mail)
        {
            var result = await MailHelper.SendMail(mail);
            if (result)
                return new APISuccessResult<bool>(Messages.SendMailSuccess, true);

            return new APIErrorResult<bool>(Messages.SendMailFailed);
        }

        #endregion
    }
}
