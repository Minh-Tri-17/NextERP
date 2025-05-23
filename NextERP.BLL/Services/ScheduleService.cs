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
    public class ScheduleService : IScheduleService
    {
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public ScheduleService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(Guid id, ScheduleModel request)
        {
            if (id == Guid.Empty)
            {
                var schedule = new Schedule();
                DataHelper.MapAudit(request, schedule, _currentUser.UserName);

                await _context.Schedules.AddAsync(schedule);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var schedule = await _context.Schedules.FindAsync(id);
                if (schedule == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, schedule, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listScheduleId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listSchedule = await _context.Schedules
                .Where(x => listScheduleId.Contains(x.Id))
                .ToListAsync();

            foreach (var schedule in listSchedule)
            {
                schedule.IsDelete = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<ScheduleModel>> GetOne(Guid id)
        {
            var schedule = await _context.Schedules
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(x => x.Id == id);

            if (schedule == null)
                return new APIErrorResult<ScheduleModel>(Messages.NotFoundGet);

            var scheduleModel = DataHelper.Mapping<Schedule, ScheduleModel>(schedule);

            return new APISuccessResult<ScheduleModel>(Messages.GetResultSuccess, scheduleModel);
        }

        public async Task<APIBaseResult<PagingResult<ScheduleModel>>> GetPaging(Filter filter)
        {
            IQueryable<Schedule> query = _context.Schedules
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.ScheduleCode)
                    && x.ScheduleCode.ToLower().Contains(keyword));
            }

            var listSchedule = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var listScheduleModel = DataHelper.MappingList<Schedule, ScheduleModel>(listSchedule);

            if (!listSchedule.Any())
                return new APIErrorResult<PagingResult<ScheduleModel>>(Messages.NotFoundGetList);

            var totalCount = await query.CountAsync();
            var pageResult = new PagingResult<ScheduleModel>()
            {
                TotalRecord = totalCount,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listScheduleModel
            };

            return new APISuccessResult<PagingResult<ScheduleModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listScheduleModel = new List<ScheduleModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);

                if (row != null)
                {
                    ScheduleModel scheduleModel = DataHelper.CopyImport<ScheduleModel>(headerRow, row);
                    listScheduleModel.Add(scheduleModel);
                }
            }

            var listSchedule = new List<Schedule>();
            DataHelper.MapListAudit<ScheduleModel, Schedule>(listScheduleModel, listSchedule, _currentUser.UserName);

            await _context.Schedules.AddRangeAsync(listSchedule);

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
            var worksheet = workbook.Worksheets.Add(ObjectNames.Schedule);

            var listSchedule = new List<Schedule>();
            DataHelper.MapAudit(items, listSchedule, string.Empty);
            DataHelper.CopyExport(worksheet, listSchedule);

            var stream = new MemoryStream();
            workbook.SaveAs(stream); // Ghi nội dung của workbook(Excel) vào stream
            var bytes = stream.ToArray(); // Chuyển toàn bộ nội dung stream thành mảng byte
            if (bytes.Length > 0)
                return new APISuccessResult<byte[]>(Messages.ExportSuccess, bytes);

            return new APIErrorResult<byte[]>(Messages.ExportFailed);
        }
    }
}
