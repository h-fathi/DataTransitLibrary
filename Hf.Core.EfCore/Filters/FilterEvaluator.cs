using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Hf.Core.EfCore.GenericRepoitory
{
    internal static class FilterEvaluator
    {
        public static IQueryable<T> GetSpecifiedQuery<T>(this IQueryable<T> inputQuery, Filter<T> filter)
            where T : class
        {
            IQueryable<T> query = GetSpecifiedQuery(inputQuery, (FilterBase<T>)filter);

            // Apply paging if enabled
            if (filter.Skip != null)
            {
                if (filter.Skip < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(filter.Skip), $"The value of {nameof(filter.Skip)} in {nameof(filter)} can not be negative.");
                }

                query = query.Skip((int)filter.Skip);
            }

            if (filter.Take != null)
            {
                if (filter.Take < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(filter.Take), $"The value of {nameof(filter.Take)} in {nameof(filter)} can not be negative.");
                }

                query = query.Take((int)filter.Take);
            }

            return query;
        }

        public static IQueryable<T> GetSpecifiedQuery<T>(this IQueryable<T> inputQuery, PaginationFilter<T> filter)
            where T : class
        {
            if (inputQuery == null)
            {
                throw new ArgumentNullException(nameof(inputQuery));
            }

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (filter.PageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(filter.PageIndex), "The value of pageIndex must be greater than 0.");
            }

            if (filter.PageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(filter.PageSize), "The value of pageSize must be greater than 0.");
            }

            IQueryable<T> query = GetSpecifiedQuery(inputQuery, (FilterBase<T>)filter);

            // Apply paging if enabled
            int skip = (filter.PageIndex - 1) * filter.PageSize;

            query = query.Skip(skip).Take(filter.PageSize);

            return query;
        }

        public static IQueryable<T> GetSpecifiedQuery<T>(this IQueryable<T> inputQuery, FilterBase<T> filter)
            where T : class
        {
            if (inputQuery == null)
            {
                throw new ArgumentNullException(nameof(inputQuery));
            }

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            IQueryable<T> query = inputQuery;

            // modify the IQueryable using the filter's criteria expression
            if (filter.Conditions != null && filter.Conditions.Any())
            {
                foreach (Expression<Func<T, bool>> filterCondition in filter.Conditions)
                {
                    query = query.Where(filterCondition);
                }
            }

            // Includes all expression-based includes
            if (filter.Includes != null)
            {
                query = filter.Includes(query);
            }

            // Apply ordering if expressions are set
            if (filter.OrderBy != null)
            {
                query = filter.OrderBy(query);
            }
            else if (!string.IsNullOrWhiteSpace(filter.OrderByDynamic.ColumnName) && !string.IsNullOrWhiteSpace(filter.OrderByDynamic.ColumnName))
            {
                query = query.OrderBy(filter.OrderByDynamic.ColumnName + " " + filter.OrderByDynamic.SortDirection);
            }

            return query;
        }
    }
}
