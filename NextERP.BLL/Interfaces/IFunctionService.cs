using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IFunctionService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(FunctionModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<FunctionModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<FunctionModel>>> GetPaging(Filter filter);
    }
}
