using System;
using System.Linq.Expressions;
using Security.Interfaces.Collections;
using Security.Linq.Mapping;

namespace Security.Linq
{
    public static class Set
    {
        public static ISet<T> Where<T>(this ISet<T> filter, Expression<Func<T, bool>> predicat)

        {
            filter.Query = filter.Query.WhereWithFilter(predicat);
            return filter;
        }

        public static ISet<T> Skip<T>(this ISet<T> filter, int count)
            where T : class
        {
            filter.Query = filter.Query.SkipWithFilter(count);
            return filter;
        }

        public static ISet<T> Take<T>(this ISet<T> filter, int count)
        {
            filter.Query = filter.Query.TakeWithFilter(count);
            return filter;
        }

        public static ISet<T> OrderBy<T, TKey>(this ISet<T> filter, Expression<Func<T, TKey>> predicat)
            where T : class
        {
            filter.Query = filter.Query.OrderByWithFilter(predicat);
            return (ISet<T>)filter;
        }

        public static ISet<T> OrderByDescending<T, TKey>(this ISet<T> filter,
            Expression<Func<T, TKey>> predicat)
        {
            filter.Query = filter.Query.OrderByDescendingWithFilter(predicat);
            return (ISet<T>)filter;
        }

        public static ISet<T> Include<T, TKey>(this ISet<T> filter, Expression<Func<T, TKey>> includePredicat)
            where T : class
        {
            filter.Query = filter.Query.IncludeWithFilter(includePredicat);
            return filter;
        }

        public static ISet<T> Include<T>(this ISet<T> filter, string pathProperty)
        {
            filter.Query = filter.Query.IncludeWithFilter(pathProperty);
            return filter;
        }

        public static ISet Include(this ISet filter, string pathProperty)
        {
            filter.Query = filter.Query.IncludeWithFilter(pathProperty);
            return filter;
        }

        public static ISet<T> OfType<T>(this ISet filter) where T : class
        {
            filter.Query = filter.Query.OfTypeWithFilter(Mapper.Instance.GetEntityType(typeof(T)));
            return new SecQueryable<T>(filter.Query);
        }

        public static T First<T>(this ISet<T> filter)
            where T : class
        {
            return (T)filter.Query.FirstWithFilter();
        }

        public static T FirstOrDefault<T>(this ISet<T> filter)
            where T : class
        {
            var entity = filter.Query.FirstOrDefaultWithFilter();
            if (entity == null)
                return null;

            return (T)entity;
        }

        public static T First<T>(this ISet<T> filter, Expression<Func<T, bool>> wherePredicat)
            where T : class
        {
            return (T)filter.Query.FirstWithFilter(wherePredicat);
        }

        public static T FirstOrDefault<T>(this ISet<T> filter, Expression<Func<T, bool>> wherePredicat)
            where T : class
        {
            return filter.Query.FirstOrDefaultWithFilter(wherePredicat);
        }

        public static object FirstOrDefault(ISet filter, LambdaExpression condition)
        {
            var entity = filter.Query.FirstOrDefaultWithFilterPrimitive(condition);
            if (entity == null)
                return null;

            return entity;
        }

        public static ISet<TResult> SelectProxy<TSource, TResult>(this ISet<TSource> filter,
            Expression<Func<TSource, TResult>> selector)
        {
            filter.Query = filter.Query.SelectWithFilter(selector);

            return new SecQueryable<TResult>(filter.Query);
        }

        public static int Count<T>(this ISet<T> filter)
        {
            return filter.Query.CountWithFilter();
        }

        public static int Count<T>(this ISet<T> filter, Expression<Func<T, bool>> wherePredicate)
        {
            return filter.Query.CountWithFilter(wherePredicate);
        }

//        public static ISet<TResult> Join<TOuter, TInner, TKey, TResult>(this ISet<TOuter> filter,
//            ISet<TInner> inner, Expression<Func<TOuter, TKey>> outKeySelector,
//            Expression<Func<TInner, TKey>> innerKeySeletor, Expression<Func<TOuter, TInner, TResult>> resultSelector)
//            where TResult : class
//        {
//            filter.Query = filter.Query.JoinWithFilter(inner, outKeySelector, innerKeySeletor, resultSelector);
//
//            return new SunoQueryable<TResult>(filter.Query);
//        }

        public static bool Any<TSourceBll>(this ISet<TSourceBll> filter)
        {
            return filter.Query.AnyWithFilter();
        }

        public static bool Any<TSourceBll>(this ISet<TSourceBll> filter,
            Expression<Func<TSourceBll, bool>> predicate)
        {
            return filter.Query.AnyWithFilter(predicate);
        }
    }
}
