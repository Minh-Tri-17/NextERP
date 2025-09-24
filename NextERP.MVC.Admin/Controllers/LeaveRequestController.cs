using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class LeaveRequestController : BaseController
    {
        #region Infrastructure

        private readonly ILeaveRequestAPIService _leaveRequestAPIService;
        private readonly ISharedCultureLocalizer _localizer;

        public LeaveRequestController(ILeaveRequestAPIService leaveRequestAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _leaveRequestAPIService = leaveRequestAPIService;
            _localizer = localizer;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult LeaveRequestIndex()
        {
            return View(new LeaveRequestModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _leaveRequestAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(LeaveRequestModel request)
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
                        FilterName = LeaveRequestModel.AttributeNames.LeaveRequestCode,
                        FilterValue = DataHelper.GetString(request.LeaveRequestCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = LeaveRequestModel.AttributeNames.ApprovalStatus,
                        FilterValue = DataHelper.GetString(request.ApprovalStatus),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Contains.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = LeaveRequestModel.AttributeNames.LeaveDayType,
                        FilterValue = DataHelper.GetString(request.LeaveDayType),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Contains.ToString(),
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

            var result = await _leaveRequestAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return PartialView(ScreenName.LeaveRequest.LeaveRequestList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(LeaveRequestModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _leaveRequestAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _leaveRequestAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return RedirectToAction(ScreenName.LeaveRequest.LeaveRequestIndex, _localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _leaveRequestAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _leaveRequestAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            return Json(_localizer.GetLocalizedString(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(LeaveRequestModel request)
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
                        FilterName = LeaveRequestModel.AttributeNames.LeaveRequestCode,
                        FilterValue = DataHelper.GetString(request.LeaveRequestCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = LeaveRequestModel.AttributeNames.ApprovalStatus,
                        FilterValue = DataHelper.GetString(request.ApprovalStatus),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Contains.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = LeaveRequestModel.AttributeNames.LeaveDayType,
                        FilterValue = DataHelper.GetString(request.LeaveDayType),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Contains.ToString(),
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

            var result = await _leaveRequestAPIService.Export(filter);
            if (!DataHelper.IsNotNull(result))
                return Json(_localizer.GetLocalizedString(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.LeaveRequest, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
