using Microsoft.AspNetCore.Mvc.Rendering;
using NextERP.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static string IsActive(this IHtmlHelper html, string controllers = "", string actions = "", string cssClassTrue = "", string cssClassFalse = "")
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
                ? cssClassTrue : cssClassFalse;
        }

        /// <summary>
        /// Trả ra list Properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excludedFields"></param>
        /// <returns></returns>
        public static IReadOnlyList<PropertyInfo> GetOrderedProperties<T>(IEnumerable<string> excludedFields)
        {
            var excluded = new HashSet<string>(excludedFields ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);

            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(s => s.Name != null && !excluded.Contains(s.Name)
                    && s.PropertyType != typeof(Guid) && s.PropertyType != typeof(Guid?)
                    && s.GetMethod != null && !s.GetMethod.IsVirtual)
                .OrderBy(p => Attribute.IsDefined(p, typeof(ViewFieldAttribute)) ? 1 : 0)
                .ToList();
        }
    }
}
