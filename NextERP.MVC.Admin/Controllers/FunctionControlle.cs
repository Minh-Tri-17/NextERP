using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class FunctionController : BaseController
    {
        private readonly IFunctionAPIService _functionAPIService;

        public FunctionController(IFunctionAPIService functionAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _functionAPIService = functionAPIService;
        }

        [HttpGet]
        public IActionResult FunctionIndex()
        {
            return View(new FunctionModel());
        }
    }
}
