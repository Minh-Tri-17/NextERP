using Microsoft.AspNetCore.Http;
using NextERP.ModelBase;
using NextERP.ModelBase.APIResult;
using NextERP.ModelBase.PagingResult;

namespace NextERP.BLL.Interface
{
    public interface ISpaServiceService
    {
        public Task<APIBaseResult<bool>> CreateOrEdit(SpaServiceModel request);
        public Task<APIBaseResult<bool>> Delete(string ids);
        public Task<APIBaseResult<SpaServiceModel>> GetOne(Guid id);
        public Task<APIBaseResult<PagingResult<SpaServiceModel>>> GetPaging(Filter filter);
        public Task<APIBaseResult<bool>> Import(IFormFile fileImport);
        public Task<APIBaseResult<byte[]>> Export(Filter filter);
    }
}
