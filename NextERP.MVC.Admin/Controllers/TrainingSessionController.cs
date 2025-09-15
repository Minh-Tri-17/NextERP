using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Mvc;
using NextERP.ModelBase;
using NextERP.MVC.Admin.Services.Interfaces;
using NextERP.MVC.Admin.Services.Services;
using NextERP.Util;

namespace NextERP.MVC.Admin.Controllers
{
    public class TrainingSessionController : BaseController
    {
        #region Infrastructure

        private readonly ITrainingSessionAPIService _trainingSessionAPIService;

        public TrainingSessionController(ITrainingSessionAPIService trainingSessionAPIService, IConfiguration configuration, ISharedCultureLocalizer localizer) : base(configuration, localizer)
        {
            _trainingSessionAPIService = trainingSessionAPIService;
        }

        #endregion

        #region Default Operations

        [HttpGet]
        public IActionResult TrainingSessionIndex()
        {
            return View(new TrainingSessionModel());
        }

        [HttpGet]
        public async Task<ActionResult> CreateOrEdit(Guid id)
        {
            var result = await _trainingSessionAPIService.GetOne(id);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(result.Result);
        }

        [HttpPost]
        public async Task<ActionResult> GetList(TrainingSessionModel request)
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
                        FilterName = AttributeNames.TrainingSession.TrainingSessionCode,
                        FilterValue = DataHelper.GetString(request.TrainingSessionCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = AttributeNames.TrainingSession.SessionStatus,
                        FilterValue = DataHelper.GetString(request.SessionStatus),
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

            var result = await _trainingSessionAPIService.GetPaging(filter);
            if (!DataHelper.ListIsNotNull(result))
                return Json(Localization(result.Message));

            return PartialView(ScreenName.TrainingSession.TrainingSessionList, result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrEdit(TrainingSessionModel request)
        {
            if (!ModelState.IsValid)
                return GetModelStateErrors();

            var result = await _trainingSessionAPIService.CreateOrEdit(request);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string ids)
        {
            var result = await _trainingSessionAPIService.Delete(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> DeletePermanently(string ids)
        {
            var result = await _trainingSessionAPIService.DeletePermanently(ids);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(Messages.FileNotFound);

            var result = await _trainingSessionAPIService.Import(file);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            return Json(Localization(result.Message));
        }

        [HttpPost]
        public async Task<ActionResult> Export(TrainingSessionModel request)
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
                        FilterName = AttributeNames.TrainingSession.TrainingSessionCode,
                        FilterValue = DataHelper.GetString(request.TrainingSessionCode),
                        FilterType = Util.Enums.FilterType.String.ToString(),
                        FilterOperator = Util.Enums.FilterOperator.Like.ToString(),
                    },
                    new FilterItemModel()
                    {
                        FilterName = AttributeNames.TrainingSession.SessionStatus,
                        FilterValue = DataHelper.GetString(request.SessionStatus),
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

            var result = await _trainingSessionAPIService.Export(filter);
            if (!DataHelper.IsNotNull(result))
                return Json(Localization(result.Message));

            var fileName = string.Format(Constants.FileName, TableName.TrainingSession, DateTime.Now.ToString(Constants.DateTimeString));
            return File(result.Result!, Constants.ContentType, fileName);
        }

        #endregion

        #region Custom Operations

        #endregion
    }
}
