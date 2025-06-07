using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase.PagingResult;
using NextERP.Util;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = Constants.Pagination)]
    public class PaginationComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagingBaseResult result)
        {
            return Task.FromResult((IViewComponentResult)View(Constants.Pagination, result));
        }
    }
}
