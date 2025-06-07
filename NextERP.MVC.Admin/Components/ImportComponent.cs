using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = "Import")]
    public class ImportComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string tableName)
        {
            return Task.FromResult((IViewComponentResult)View("Import", tableName));
        }
    }
}
