using Microsoft.EntityFrameworkCore;
using NextERP.ModelBase;
using System.Linq.Expressions;

namespace NextERP.Util
{
    public static class QueryableFilterExtensions
    {
        public static IQueryable<T> ApplyCommonFilters<T>(
            this IQueryable<T> query,
            Filter filter,
            Expression<Func<T, string>> codeSelector,
            Expression<Func<T, bool?>> isDeleteSelector,
            Expression<Func<T, Guid>> idSelector)
        {
            if (filter.IsNotDelete)
            {
                // Truy xuất tên property từ biểu thức isDeleteSelector
                var isDeletePropName = ((MemberExpression)isDeleteSelector.Body).Member.Name;
                query = query.Where(s => EF.Property<bool?>(s, isDeletePropName) != true);
            }

            if (!string.IsNullOrEmpty(filter.KeyWord))
            {
                var keyword = filter.KeyWord.Trim().ToLower();
                var codePropName = ((MemberExpression)codeSelector.Body).Member.Name;
                query = query.Where(s =>
                    EF.Functions.Like(
                        EF.Property<string>(s, codePropName).ToLower(),
                        $"%{keyword}%"
                    ));
            }

            if (!string.IsNullOrEmpty(filter.Ids))
            {
                var listIds = filter.Ids.Split(',')
                    .Select(id => DataHelper.GetGuid(id.Trim()))
                    .Where(guid => guid != Guid.Empty)
                    .ToList();

                var idPropName = ((MemberExpression)idSelector.Body).Member.Name;
                query = query.Where(s => listIds.Contains(EF.Property<Guid>(s, idPropName)));
            }

            return query;
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, Filter filter)
        {
            if (filter.AllowPaging)
            {
                query = query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
            }

            return query;
        }
    }
}
