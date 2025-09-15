using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface IAppointmentService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(AppointmentModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<AppointmentModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<AppointmentModel>>> GetPaging(FilterModel filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(FilterModel filter);
    }
}
