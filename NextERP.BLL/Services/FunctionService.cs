﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NextERP.BLL.Interface;
using NextERP.DAL.Models;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;
using NextERP.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.BLL.Service
{
    public class FunctionService : IFunctionService
    {
        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public FunctionService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<APIBaseResult<bool>> CreateOrEdit(Guid id, FunctionModel request)
        {
            if (id == Guid.Empty)
            {
                var function = new Function();
                DataHelper.MapAudit(request, function, _currentUser.UserName);

                await _context.Functions.AddAsync(function);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.CreateSuccess, true);

                return new APIErrorResult<bool>(Messages.CreateFailed);
            }
            else
            {
                var function = await _context.Functions.FindAsync(id);
                if (function == null)
                    return new APIErrorResult<bool>(Messages.NotFoundUpdate);

                DataHelper.MapAudit(request, function, _currentUser.UserName);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return new APISuccessResult<bool>(Messages.UpdateSuccess, true);

                return new APIErrorResult<bool>(Messages.UpdateFailed);
            }
        }

        public async Task<APIBaseResult<bool>> Delete(string ids)
        {
            List<Guid> listFunctionId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listFunction = await _context.Functions
                .Where(x => listFunctionId.Contains(x.Id))
                .ToListAsync();

            foreach (var function in listFunction)
            {
                function.IsDelete = true;
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<FunctionModel>> GetOne(Guid id)
        {
            var function = await _context.Functions
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .FirstOrDefaultAsync(x => x.Id == id);

            if (function == null)
                return new APIErrorResult<FunctionModel>(Messages.NotFoundGet);

            var functionModel = DataHelper.Mapping<Function, FunctionModel>(function);

            return new APISuccessResult<FunctionModel>(Messages.GetResultSuccess, functionModel);
        }

        public async Task<APIBaseResult<PagingResult<FunctionModel>>> GetPaging(Filter filter)
        {
            IQueryable<Function> query = _context.Functions
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(x => x.IsDelete != true);

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();

                query = query.Where(x => !string.IsNullOrEmpty(x.FunctionCode)
                    && x.FunctionCode.ToLower().Contains(keyword));
            }

            var listFunction = await query
                .OrderByDescending(x => x.DateUpdate ?? x.DateCreate)
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var listFunctionModel = DataHelper.MappingList<Function, FunctionModel>(listFunction);

            if (!listFunction.Any())
                return new APIErrorResult<PagingResult<FunctionModel>>(Messages.NotFoundGetList);

            var totalCount = await query.CountAsync();
            var pageResult = new PagingResult<FunctionModel>()
            {
                TotalRecord = totalCount,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listFunctionModel
            };

            return new APISuccessResult<PagingResult<FunctionModel>>(Messages.GetListResultSuccess, pageResult);
        }
    }
}
