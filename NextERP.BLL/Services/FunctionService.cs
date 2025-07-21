using Microsoft.AspNetCore.Http;
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
        #region Infrastructure

        private readonly NextErpContext _context; // Dùng để truy cập vào DbContext
        private readonly ICurrentUserService _currentUser; // Dùng để lấy thông tin người dùng hiện tại

        public FunctionService(NextErpContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        #endregion

        #region Default Operations

        public async Task<APIBaseResult<bool>> CreateOrEdit(FunctionModel request)
        {
            #region Check null request and create variable

            var id = DataHelper.GetGuid(request.Id);

            #endregion

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
                .Where(s => listFunctionId.Contains(s.Id))
                .ToListAsync();

            foreach (var function in listFunction)
            {
                function.IsDelete = true; // Đánh dấu là đã xóa
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return new APISuccessResult<bool>(Messages.DeleteSuccess, true);

            return new APIErrorResult<bool>(Messages.DeleteFailed);
        }

        public async Task<APIBaseResult<bool>> DeletePermanently(string ids)
        {
            List<Guid> listFunctionId = ids.Split(',')
                .Select(id => DataHelper.GetGuid(id.Trim()))
                .Where(guid => guid != Guid.Empty)
                .ToList();

            var listFunction = await _context.Functions
                .Where(s => listFunctionId.Contains(s.Id))
                .ToListAsync();

            foreach (var function in listFunction)
            {
                _context.Functions.Remove(function); // Xóa vĩnh viễn
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
                .FirstOrDefaultAsync(s => s.Id == id);

            if (function == null)
                return new APIErrorResult<FunctionModel>(Messages.NotFoundGet);

            var functionModel = DataHelper.Mapping<Function, FunctionModel>(function);

            return new APISuccessResult<FunctionModel>(Messages.GetResultSuccess, functionModel);
        }

        public async Task<APIBaseResult<PagingResult<FunctionModel>>> GetPaging(Filter filter)
        {
            IQueryable<Function> query = _context.Functions
                .AsNoTracking() // Không theo dõi thay đổi của thực thể
                .Where(s => s.IsDelete != true);

            var listFunction = await query
                .OrderByDescending(s => s.DateUpdate ?? s.DateCreate)
                .ToListAsync();

            var listFunctionModel = DataHelper.MappingList<Function, FunctionModel>(listFunction);
            var pageResult = new PagingResult<FunctionModel>()
            {
                TotalRecord = await query.CountAsync(),
                PageRecord = listFunctionModel.Count(),
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                Items = listFunctionModel
            };

            return new APISuccessResult<PagingResult<FunctionModel>>(Messages.GetListResultSuccess, pageResult);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
