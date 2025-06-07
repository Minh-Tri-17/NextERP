using Microsoft.AspNetCore.Mvc;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = "Loading")]
    public class LoadingComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult((IViewComponentResult)View("Loading"));
        }
    }
}
