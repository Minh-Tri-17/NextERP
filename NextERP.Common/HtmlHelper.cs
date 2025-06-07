using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.Util
{
    public static class HtmlHelper
    {
        /// <summary>
        /// Trả về chuỗi CSS class nếu controller và action hiện tại khớp với tham số truyền vào.
        /// Có thể dùng để đánh dấu menu item và submenu đang hoạt động hoặc submenu mở rộng (collapse).
        /// </summary>
        /// <param name="html"></param>
        /// <param name="controllers"></param>
        /// <param name="actions"></param>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public static string IsActive(this IHtmlHelper html, string controllers = "", string actions = "", string cssClass = "")
        {
            // Lấy giá trị action và controller hiện tại từ RouteData
            var currentAction = html.ViewContext.RouteData.Values["action"]?.ToString() ?? string.Empty;
            var currentController = html.ViewContext.RouteData.Values["controller"]?.ToString() ?? string.Empty;

            var acceptedActions = string.IsNullOrWhiteSpace(actions)
                ? new[] { currentAction }
                : actions.Split(',').Select(a => a.Trim());

            var acceptedControllers = string.IsNullOrWhiteSpace(controllers)
                ? new[] { currentController }
                : controllers.Split(',').Select(c => c.Trim());

            // Trả về cssClass nếu controller và action hiện tại có trong danh sách hợp lệ
            return acceptedControllers.Contains(currentController) && acceptedActions.Contains(currentAction)
                ? cssClass : string.Empty;
        }
    }
}
