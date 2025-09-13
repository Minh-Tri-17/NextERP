using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Services.Interfaces
{
    public interface IHistoryMailAPIService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(HistoryMailModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<bool>> DeletePermanently(string ids);
        public Task<APIBaseResult<HistoryMailModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<HistoryMailModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
