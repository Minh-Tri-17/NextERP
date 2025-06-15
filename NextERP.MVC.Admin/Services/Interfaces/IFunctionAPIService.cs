using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface IFunctionAPIService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(FunctionModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<FunctionModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<FunctionModel>>> GetPaging(Filter filter);
    }
}
