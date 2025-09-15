using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class TemplateNotificationController : BaseController
    {
        #region Infrastructure

        private readonly ITemplateNotificationAPIService _templateNotificationAPIService;

        public TemplateNotificationController(ITemplateNotificationAPIService templateNotificationAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _templateNotificationAPIService = templateNotificationAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult TemplateNotificationIndex()
        {
            return View(new TemplateNotificationModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _templateNotificationAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(TemplateNotificationModel request)
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
                        FilterName = AttributeNames.TemplateNotification.TemplateNotificationCode,
                        FilterValue = DataHelper.GetString(request.TemplateNotificationCode),
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

            var result = await _templateNotificationAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(Localization(result.Message));

            return PartialView(ScreenName.TemplateNotification.TemplateNotificationList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(TemplateNotificationModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _templateNotificationAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _templateNotificationAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _templateNotificationAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _templateNotificationAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(TemplateNotificationModel request)
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
                        FilterName = AttributeNames.TemplateNotification.TemplateNotificationCode,
                        FilterValue = DataHelper.GetString(request.TemplateNotificationCode),
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

            var result = await _templateNotificationAPIService.Export(filter);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.TemplateNotification, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
