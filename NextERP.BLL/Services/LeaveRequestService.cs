﻿using ClosedXML.Excel;
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
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public LeaveRequestService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(Guid id, LeaveRequestModel request)
        {
            if (id == Guid.Empty)
            {
                var leaveRequest = new LeaveRequest();
                DataHelper.MapAudit(request, leaveRequest, _currentUser.UserName);

                await _context.LeaveRequests.AddAsync(leaveRequest);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var leaveRequest = await _context.LeaveRequests.FindAsync(id);
                if (leaveRequest == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, leaveRequest, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listLeaveRequestId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listLeaveRequest = await _context.LeaveRequests
                .Where(x => listLeaveRequestId.Contains(x.Id))
                .ToListAsync();

            foreach (var leaveRequest in listLeaveRequest)
            {
                leaveRequest.IsDelete = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<LeaveRequestModel>> GetOne(Guid id)
        {
            var leaveRequest = await _context.LeaveRequests
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(x => x.Id == id);

            if (leaveRequest == null)
                return new APIErrorResult<LeaveRequestModel>(Messages.NotFoundGet);

            var leaveRequestModel = DataHelper.Mapping<LeaveRequest, LeaveRequestModel>(leaveRequest);

            return new APISuccessResult<LeaveRequestModel>(Messages.GetResultSuccess, leaveRequestModel);
        }

        public async Task<APIBaseResult<PagingResult<LeaveRequestModel>>> GetPaging(Filter filter)
        {
            IQueryable<LeaveRequest> query = _context.LeaveRequests
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.LeaveRequestCode)
                    && x.LeaveRequestCode.ToLower().Contains(keyword));
            }

            var listLeaveRequest = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var listLeaveRequestModel = DataHelper.MappingList<LeaveRequest, LeaveRequestModel>(listLeaveRequest);

            if (!listLeaveRequest.Any())
                return new APIErrorResult<PagingResult<LeaveRequestModel>>(Messages.NotFoundGetList);

            var totalCount = await query.CountAsync();
            var pageResult = new PagingResult<LeaveRequestModel>()
            {
                TotalRecord = totalCount,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listLeaveRequestModel
            };

            return new APISuccessResult<PagingResult<LeaveRequestModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listLeaveRequestModel = new List<LeaveRequestModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);

                if (row != null)
                {
                    LeaveRequestModel leaveRequestModel = DataHelper.CopyImport<LeaveRequestModel>(headerRow, row);
                    listLeaveRequestModel.Add(leaveRequestModel);
                }
            }

            var listLeaveRequest = new List<LeaveRequest>();
            DataHelper.MapListAudit<LeaveRequestModel, LeaveRequest>(listLeaveRequestModel, listLeaveRequest, _currentUser.UserName);

            await _context.LeaveRequests.AddRangeAsync(listLeaveRequest);

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
            var worksheet = workbook.Worksheets.Add(ObjectNames.LeaveRequest);

            var listLeaveRequest = new List<LeaveRequest>();
            DataHelper.MapAudit(items, listLeaveRequest, string.Empty);
            DataHelper.CopyExport(worksheet, listLeaveRequest);

            var stream = new MemoryStream();
            workbook.SaveAs(stream); // Ghi nội dung của workbook(Excel) vào stream
            var bytes = stream.ToArray(); // Chuyển toàn bộ nội dung stream thành mảng byte
            if (bytes.Length > 0)
                return new APISuccessResult<byte[]>(Messages.ExportSuccess, bytes);

            return new APIErrorResult<byte[]>(Messages.ExportFailed);
        }
    }
}
