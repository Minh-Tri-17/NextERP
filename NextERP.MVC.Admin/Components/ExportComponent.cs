using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = "Export")]
    public class ExportComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string tableName)
        {
            return Task.FromResult((IViewComponentResult)View("Export", tableName));
        }
    }
}
