using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class TemplateMailController : BaseController
    {
        #region Infrastructure

        private readonly ITemplateMailAPIService _templateMailAPIService;
        private readonly ISharedCultureLocalizer _localizer;

        public TemplateMailController(ITemplateMailAPIService templateMailAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _templateMailAPIService = templateMailAPIService;
            _localizer = localizer;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult TemplateMailIndex()
        {
            return View(new TemplateMailModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _templateMailAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(TemplateMailModel request)
        {
            FilterModel filter = new FilterModel()
            {
                Filters = new List<FilterItemModel>()
                {
                    new FilterItemModel()
                    {
                        FilterName = Constants.IsDelete,
                        FilterType = Util.Enums.FilterType.Boolean.ToString(),
                        FilterOperator = DataHelper.GetBool(request.IsDelete)
                            ? Util.Enums.FilterOperator.Equal.ToString()
                            : Util.Enums.FilterOperator.NotEqual.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = TemplateMailModel.AttributeNames.TemplateMailCode,
                        FilterValue = DataHelper.GetString(request.TemplateMailCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = Constants.DateCreate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.DateCreate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = Constants.DateUpdate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.DateUpdate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                },
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };

            var result = await _templateMailAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return PartialView(ScreenName.TemplateMail.TemplateMailList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(TemplateMailModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _templateMailAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _templateMailAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _templateMailAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _templateMailAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(TemplateMailModel request)
        {
            FilterModel filter = new FilterModel()
            {
                Filters = new List<FilterItemModel>()
                {
                    new FilterItemModel()
                    {
                        FilterName = Constants.IsDelete,
                        FilterType = Util.Enums.FilterType.Boolean.ToString(),
                        FilterOperator = DataHelper.GetBool(request.IsDelete)
                            ? Util.Enums.FilterOperator.Equal.ToString()
                            : Util.Enums.FilterOperator.NotEqual.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = Constants.Ids,
                        FilterValue = DataHelper.GetString(request.Ids),
                        FilterType = Util.Enums.FilterType.Guid.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Contains.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = TemplateMailModel.AttributeNames.TemplateMailCode,
                        FilterValue = DataHelper.GetString(request.TemplateMailCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = Constants.DateCreate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.DateCreate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = Constants.DateUpdate,
                        FilterValue = DataHelper.GetString(DataHelper.GetDateTime(request.DateUpdate)),
                        FilterType = Util.Enums.FilterType.Date.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Equal.ToString(),
                    },
                },
                AllowPaging = request.AllowPaging,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };

            var result = await _templateMailAPIService.Export(filter);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.TemplateMail, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        [HttpPost]
        public async Task<ActionResult> SendMail(MailModel mail)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            mail.CC = DataHelper.GetString(mail.CC.FirstOrDefault()).Split(",").ToList();

            var result = await _templateMailAPIService.SendMail(mail);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        #endregion
    }
}
