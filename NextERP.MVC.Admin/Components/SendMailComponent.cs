using Microsoft.AspNetCore.Mvc;
using NextERP.Util;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = Constants.SendMail)]
    public class SendMailComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string tableName)
        {
            return Task.FromResult((IViewComponentResult)View(Constants.SendMail, tableName));
        }
    }
}
