﻿using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class FunctionController : BaseController
    {
        #region Infrastructure

        private readonly IFunctionAPIService _functionAPIService;

        public FunctionController(IFunctionAPIService functionAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _functionAPIService = functionAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult FunctionIndex()
        {
            return View(new FunctionModel());
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
