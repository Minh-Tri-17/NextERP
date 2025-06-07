using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface ILeaveRequestAPIService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(LeaveRequestModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<LeaveRequestModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<LeaveRequestModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
