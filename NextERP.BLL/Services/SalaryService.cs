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
    public class SalaryService : ISalaryService
    {
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public SalaryService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(Guid id, SalaryModel request)
        {
            if (id == Guid.Empty)
            {
                var salary = new Salary();
                DataHelper.MapAudit(request, salary, _currentUser.UserName);

                await _context.Salaries.AddAsync(salary);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var salary = await _context.Salaries.FindAsync(id);
                if (salary == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, salary, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listSalaryId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listSalary = await _context.Salaries
                .Where(x => listSalaryId.Contains(x.Id))
                .ToListAsync();

            foreach (var salary in listSalary)
            {
                salary.IsDelete = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<SalaryModel>> GetOne(Guid id)
        {
            var salary = await _context.Salaries
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(x => x.Id == id);

            if (salary == null)
                return new APIErrorResult<SalaryModel>(Messages.NotFoundGet);

            var salaryModel = DataHelper.Mapping<Salary, SalaryModel>(salary);

            return new APISuccessResult<SalaryModel>(Messages.GetResultSuccess, salaryModel);
        }

        public async Task<APIBaseResult<PagingResult<SalaryModel>>> GetPaging(Filter filter)
        {
            IQueryable<Salary> query = _context.Salaries
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.SalaryCode)
                    && x.SalaryCode.ToLower().Contains(keyword));
            }

            var listSalary = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var listSalaryModel = DataHelper.MappingList<Salary, SalaryModel>(listSalary);

            if (!listSalary.Any())
                return new APIErrorResult<PagingResult<SalaryModel>>(Messages.NotFoundGetList);

            var totalCount = await query.CountAsync();
            var pageResult = new PagingResult<SalaryModel>()
            {
                TotalRecord = totalCount,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listSalaryModel
            };

            return new APISuccessResult<PagingResult<SalaryModel>>(Messages.GetListResultSuccess, pageResult);
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

            var listSalaryModel = new List<SalaryModel>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                // Row data
                var row = sheet.GetRow(i);

                if (row != null)
                {
                    SalaryModel salaryModel = DataHelper.CopyImport<SalaryModel>(headerRow, row);
                    listSalaryModel.Add(salaryModel);
                }
            }

            var listSalary = new List<Salary>();
            DataHelper.MapListAudit<SalaryModel, Salary>(listSalaryModel, listSalary, _currentUser.UserName);

            await _context.Salaries.AddRangeAsync(listSalary);

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
            var worksheet = workbook.Worksheets.Add(ObjectNames.Salary);

            var listSalary = new List<Salary>();
            DataHelper.MapAudit(items, listSalary, string.Empty);
            DataHelper.CopyExport(worksheet, listSalary);

            var stream = new MemoryStream();
            workbook.SaveAs(stream); // Ghi nội dung của workbook(Excel) vào stream
            var bytes = stream.ToArray(); // Chuyển toàn bộ nội dung stream thành mảng byte
            if (bytes.Length > 0)
                return new APISuccessResult<byte[]>(Messages.ExportSuccess, bytes);

            return new APIErrorResult<byte[]>(Messages.ExportFailed);
        }
    }
}
