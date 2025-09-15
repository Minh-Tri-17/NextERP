using Microsoft.EntityFrameworkCore;
using NextERP.ModelBase;
using System.Linq.Expressions;
using static NextERP.Util.Enums;

namespace NextERP.Util
{
    public static class QueryableFilterExtensions
    {
        public static IQueryable<T> ApplyCommonFilters<T>(this IQueryable<T> query, FilterModel filter)
        {
            foreach (var item in filter.Filters)
            {
                var propName = item.FilterName;
                var propType = item.FilterType;
                var propOperator = item.FilterOperator;
                var filterValue = item.FilterValue;

                if (string.IsNullOrEmpty(propName))
                    continue;

                switch (propType, propOperator)
                {
                    case (nameof(FilterType.String), nameof(FilterOperator.Like)):
                        {
                            var propValue = DataHelper.GetString(filterValue);
                            if (!string.IsNullOrWhiteSpace(propValue))
                                query = query.Where(s => EF.Functions.Like(EF.Property<string>(s, propName).ToLower(),
                                    $"%{propValue.Trim().ToLower()}%"));

                            break;
                        }
                    case (nameof(FilterType.String), nameof(FilterOperator.Contains)):
                        {
                            var propValue = DataHelper.GetString(filterValue);
                            if (!string.IsNullOrWhiteSpace(propValue))
                                query = query.Where(s => EF.Property<string>(s, propName).Contains(propValue));

                            break;
                        }
                    case (nameof(FilterType.Guid), nameof(FilterOperator.Contains)):
                        {
                            var listGuids = DataHelper.GetString(filterValue)
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(id => DataHelper.GetGuid(id.Trim()))
                                .Where(guid => guid != Guid.Empty)
                                .ToList();

                            if (listGuids.Any())
                                query = query.Where(s => listGuids.Contains(EF.Property<Guid>(s, Constants.Id)));

                            break;
                        }
                    case (nameof(FilterType.Date), nameof(FilterOperator.Equal)):
                        {
                            var propValue = DataHelper.GetDateTime(filterValue);
                            if (propValue != DateTime.MinValue)
                                query = query.Where(s => EF.Functions.DateDiffDay(EF.Property<DateTime?>(s, propName).Value,
                                    propValue) == 0);

                            break;
                        }
                    case (nameof(FilterType.Number), nameof(FilterOperator.Equal)):
                        {
                            var propValue = DataHelper.GetInt(filterValue);
                            if (propValue != 0)
                                query = query.Where(s => EF.Property<int?>(s, propName) == propValue);

                            break;
                        }
                    case (nameof(FilterType.Boolean), nameof(FilterOperator.Equal)):
                        {
                            query = query.Where(s => EF.Property<bool?>(s, propName) == true);

                            break;
                        }
                    case (nameof(FilterType.Boolean), nameof(FilterOperator.NotEqual)):
                        {
                            query = query.Where(s => EF.Property<bool?>(s, propName) != true);

                            break;
                        }
                }
            }

            return query;
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, FilterModel filter)
        {
            if (filter.AllowPaging)
            {
                query = query.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize);
            }

            return query;
        }
    }
}
