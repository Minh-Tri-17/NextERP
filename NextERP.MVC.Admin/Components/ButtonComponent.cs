using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.Util;

namespace NextERP.MVC.Admin.Components
{
    [ViewComponent(Name = Constants.Button)]
    public class ButtonComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string nameButton, string iconHtml)
        {
            var model = new ButtonComponentViewModel
            {
                NameButton = nameButton,
                IconHtml = iconHtml
            };

            return Task.FromResult((IViewComponentResult)View(Constants.Button, model));
        }
    }
}
