using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Itis.ProxyDataPlatform.Base;
using Itis.ProxyDataPlatform.Converting;

namespace Itis.ProxyDataPlatform.Linq
{
    public static class MapQueryable
    {
        public static IMapQueryable<T> Where<T>(this IMapQueryable<T> filter, Expression<Func<T, bool>> predicat) 
            
        {
            filter.Query = filter.Query.WhereWithFilter(predicat); 
            return filter;
        }

        public static IMapQueryable<T> Skip<T>(this IMapQueryable<T> filter, int count) 
            where T : class
        {
            filter.Query = filter.Query.SkipWithFilter(count);
            return filter;
        }

        public static IMapQueryable<T> Take<T>(this IMapQueryable<T> filter, int count)
        {
            filter.Query = filter.Query.TakeWithFilter(count);
            return filter;
        }

        public static IMapOrderedQueryable<T> OrderBy<T, TKey>(this IMapQueryable<T> filter, Expression<Func<T, TKey>> predicat) 
            where T : class
        {
            filter.Query = filter.Query.OrderByWithFilter(predicat);
            return (IMapOrderedQueryable<T>) filter;
        }

        public static IMapOrderedQueryable<T> OrderByDescending<T, TKey>(this IMapQueryable<T> filter,
            Expression<Func<T, TKey>> predicat)
        {
            filter.Query = filter.Query.OrderByDescendingWithFilter(predicat);
            return (IMapOrderedQueryable<T>)filter;
        }

        public static IMapQueryable<T> Include<T, TKey>(this IMapQueryable<T> filter, Expression<Func<T, TKey >> includePredicat)
            where T : class
        {
            filter.Query = filter.Query.IncludeWithFilter(includePredicat);
            return filter;
        }

        public static IMapQueryable<T> Include<T>(this IMapQueryable<T> filter, string pathProperty)
        {
            filter.Query = filter.Query.IncludeWithFilter(pathProperty);
            return filter;
        }

        public static IMapQueryable Include(this IMapQueryable filter, string pathProperty)
        {
            filter.Query = filter.Query.IncludeWithFilter(pathProperty);
            return filter;
        }

        public static IMapQueryable<T> OfType<T>(this IMapQueryable filter) where T : class
        {
            filter.Query = filter.Query.OfTypeWithFilter(Converter.BllTypeMap(typeof(T)));
            return new SunoQueryable<T>(filter.Query);
        }

        public static T First<T>(this IMapQueryable<T> filter)
            where T : class
        {
            return Converter.Convert<T>(filter.Query.FirstWithFilter());
        }

        public static T FirstOrDefault<T>(this IMapQueryable<T> filter)
            where T : class 
        {
            var entity = filter.Query.FirstOrDefaultWithFilter();
            if (entity == null)
                return null;

            return Converter.Convert<T>(entity);
        }

        public static T First<T>(this IMapQueryable<T> filter, Expression<Func<T, bool>> wherePredicat)
            where T : class 
        {
            return Converter.Convert<T>(filter.Query.FirstWithFilter(wherePredicat));
        }

        public static T FirstOrDefault<T>(this IMapQueryable<T> filter, Expression<Func<T, bool>> wherePredicat)
            where T : class
        {
            return filter.Query.FirstOrDefaultWithFilter(wherePredicat);
        }

        public static object FirstOrDefault(IMapQueryable filter, LambdaExpression condition)
        {
            var entity = filter.Query.FirstOrDefaultWithFilterPrimitive(condition);
            if (entity == null)
                return null;

            return Converter.Convert(entity);
        }

        public static Firstable<T> FirstInQuery<T>(this IMapQueryable<T> filter)
            where T : class 
        {
            return new Firstable<T>(filter.Query);
        }

        public static Firstable<T> Include<T>(this Firstable<T> filter, string propertyName) 
            where T : class
        {
            filter.Query = filter.Query.IncludeWithFilter(propertyName);
            return filter;
        }

        public static Firstable<T> Where<T>(this Firstable<T> filter, Expression<Func<T, bool>> predicat) 
            where T : class
        {
            filter.Query = filter.Query.WhereWithFilter(predicat);
            return filter;
        }

        public static IMapQueryable<TResult> SelectProxy<TSource, TResult>(this IMapQueryable<TSource> filter,
            Expression<Func<TSource, TResult>> selector)
        {
            filter.Query = filter.Query.SelectWithFilter(selector);

            return new SunoQueryable<TResult>(filter.Query);
        }

        public static int Count<T>(this IMapQueryable<T> filter)
        {
            return filter.Query.CountWithFilter();
        }

        public static int Count<T>(this IMapQueryable<T> filter, Expression<Func<T, bool>> wherePredicate)
        {
            return filter.Query.CountWithFilter(wherePredicate);
        }

        public static IMapQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IMapQueryable<TOuter> filter,
            IMapQueryable<TInner> inner, Expression<Func<TOuter, TKey>> outKeySelector,
            Expression<Func<TInner, TKey>> innerKeySeletor, Expression<Func<TOuter, TInner, TResult>> resultSelector)
            where TResult : class
        {
            filter.Query = filter.Query.JoinWithFilter(inner, outKeySelector, innerKeySeletor, resultSelector);

            return new SunoQueryable<TResult>(filter.Query);
        }

        public static bool Any<TSourceBll>(this IMapQueryable<TSourceBll> filter)
        {
            return filter.Query.AnyWithFilter();
        }

        public static bool Any<TSourceBll>(this IMapQueryable<TSourceBll> filter,
            Expression<Func<TSourceBll, bool>> predicate)
        {
            return filter.Query.AnyWithFilter(predicate);
        }
    }
}