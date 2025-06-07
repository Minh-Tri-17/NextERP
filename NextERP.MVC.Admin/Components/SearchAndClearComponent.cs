using Microsoft.AspNetCore.Mvc;
using NextERP.Util;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = Constants.SearchBox)]
    public class SearchAndClearComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string showClass)
        {
            return Task.FromResult((IViewComponentResult)View(Constants.SearchBox, showClass));
        }
    }
}
