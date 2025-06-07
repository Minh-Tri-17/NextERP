using Microsoft.AspNetCore.Mvc;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = "SearchAndClear")]
    public class SearchAndClearComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string showClass)
        {
            return Task.FromResult((IViewComponentResult)View("SearchAndClear", showClass));
        }
    }
}
