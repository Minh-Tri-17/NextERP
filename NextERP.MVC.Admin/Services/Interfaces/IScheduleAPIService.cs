using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface IScheduleAPIService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(ScheduleModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<ScheduleModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<ScheduleModel>>> GetPaging(ScheduleModel request);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(ScheduleModel request);
    }
}
