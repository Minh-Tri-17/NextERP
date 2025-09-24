using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class RoleController : BaseController
    {
        #region Infrastructure

        private readonly IRoleAPIService _roleAPIService;
        private readonly ISharedCultureLocalizer _localizer;

        public RoleController(IRoleAPIService roleAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _roleAPIService = roleAPIService;
            _localizer = localizer;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult RoleIndex()
        {
            return View(new RoleModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _roleAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(RoleModel request)
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
                        FilterName = RoleModel.AttributeNames.RoleCode,
                        FilterValue = DataHelper.GetString(request.RoleCode),
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

            var result = await _roleAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            if (result.Result?.Items != null)
            {
                var listPermission = new List<string>();

                foreach (var role in result.Result.Items)
                {
                    listPermission = role.Permissions.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
                    role.Permissions = string.Join("<br/>", listPermission.Select(s => "&#10031; " + _localizer.GetLocalizedString(s)));
                }
            }

            return PartialView(ScreenName.Role.RoleList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(RoleModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _roleAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _roleAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _roleAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _roleAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(RoleModel request)
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
                        FilterName = RoleModel.AttributeNames.RoleCode,
                        FilterValue = DataHelper.GetString(request.RoleCode),
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

            var result = await _roleAPIService.Export(filter);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.Role, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
