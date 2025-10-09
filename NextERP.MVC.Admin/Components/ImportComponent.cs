using Microsoft.AspNetCore.Mvc;
using NextERP.Util;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = Constants.Import)]
    public class ImportComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string tableName)
        {
            return Task.FromResult((IViewComponentResult)View(Constants.Import, tableName));
        }
    }
}
