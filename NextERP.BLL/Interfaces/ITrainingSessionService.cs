using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface ITrainingSessionService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(TrainingSessionModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<TrainingSessionModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<TrainingSessionModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
