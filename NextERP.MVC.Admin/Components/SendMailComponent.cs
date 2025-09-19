using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase.PagingResult;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = "SendMail")]
    public class SendMailComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string tableName)
        {
            return Task.FromResult((IViewComponentResult)View("SendMail", tableName));
        }
    }
}
