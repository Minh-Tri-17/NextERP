using Microsoft.AspNetCore.Mvc;
using NextERP.MVC.Admin.Services.Interfaces;

namespace NextERP.MVC.Admin.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IAdminAPIService _adminAPIService;

        public AdminController(IAdminAPIService adminAPIService, IConfiguration configuration) : base(configuration)
        {
            _adminAPIService = adminAPIService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.UserId = GetInfor();
            return View();
        }
    }
}
