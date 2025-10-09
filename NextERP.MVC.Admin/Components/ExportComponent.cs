using Microsoft.AspNetCore.Mvc;
using NextERP.Util;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = Constants.Export)]
    public class ExportComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string tableName)
        {
            return Task.FromResult((IViewComponentResult)View(Constants.Export, tableName));
        }
    }
}
