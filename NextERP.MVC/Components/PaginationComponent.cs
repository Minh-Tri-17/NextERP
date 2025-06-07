using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = "Pagination")]
    public class PaginationComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagingBaseResult result)
        {
            return Task.FromResult((IViewComponentResult)View("Pagination", result));
        }
    }
}
