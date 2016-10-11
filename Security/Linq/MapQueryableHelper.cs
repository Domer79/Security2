using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Itis.Common.Extensions;
using Security.Infrastructure.Exceptions;

namespace Security.Linq
{
    internal static class MapQueryableHelper
    {
        internal static IQueryable WhereWithFilter<T>(this IQueryable query, Expression<Func<T, bool>> whereExpression)
        {
            var callExpression = Expression.Call(typeof(Queryable), "Where", new[] { query.ElementType },
                query.Expression, MemberTypeReplacement.ReplaceParameterType(query.ElementType, whereExpression));
            return query.Provider.CreateQuery(callExpression);
        }

        internal static IQueryable SkipWithFilter(this IQueryable query, int count)
        {
//            Queryable.Skip()
            var method = typeof (Queryable).GetMethod("Skip").MakeGenericMethod(query.ElementType);
            return (IQueryable) method.Invoke(null, new[] {(object) query, count});
        }

        internal static IQueryable TakeWithFilter(this IQueryable query, int count)
        {
            var method = typeof(Queryable).GetMethod("Take").MakeGenericMethod(query.ElementType);
            return (IQueryable) method.Invoke(null, new object[] {query, count});
        }

        internal static IQueryable OrderByWithFilter<T, TKey>(this IQueryable query, Expression<Func<T, TKey>> predicat)
        {
            var callExpression = Expression.Call(typeof (Queryable), "OrderBy", new[] {query.ElementType, typeof (TKey)}, 
                query.Expression, MemberTypeReplacement.ReplaceParameterType(query.ElementType, predicat));

            return query.Provider.CreateQuery(callExpression);
        }

        internal static IQueryable OrderByDescendingWithFilter<T, TKey>(this IQueryable query,
            Expression<Func<T, TKey>> predicat)
        {
            var callExpression = Expression.Call(typeof(Queryable), "OrderByDescending", new[] { query.ElementType, typeof(TKey) },
                query.Expression, MemberTypeReplacement.ReplaceParameterType(query.ElementType, predicat));

            return query.Provider.CreateQuery(callExpression);  
        }

        internal static IQueryable IncludeWithFilter<T, TProperty>(this IQueryable query,
            Expression<Func<T, TProperty>> includePredicat)
        {
            string path;
            var exp = includePredicat.Body as MemberExpression;
            if (exp != null)
                path = exp.Member.Name;
            else
            {
                throw new Exception("недопустимое лямбда выражение");
            }
            return IncludeWithFilter(query, path);
        }

        internal static IQueryable IncludeWithFilter(this IQueryable query, string propertyPath)
        {
            var method = typeof (QueryableExtensions).GetMethod("Include",
                new[] {query.GetType(), typeof (string)});

            return (IQueryable) method.Invoke(null, new object[] {query, propertyPath});
        }

        internal static IQueryable OfTypeWithFilter(this IQueryable query, Type entityType)
        {
            var callExpression = Expression.Call(typeof(Queryable), "OfType", new []{entityType}, query.Expression);
            return query.Provider.CreateQuery(callExpression);
        }

        internal static object FirstOrDefaultWithFilter(this IQueryable query)
        {
            try
            {
                return FirstWithFilter(query);
            }
            catch (SequenceEmptyException)
            {
                return null;
            }
        }

        internal static T FirstOrDefaultWithFilter<T>(this IQueryable query, Expression<Func<T, bool>> predicat) where T : class 
        {
            try
            {
                var @object = FirstWithFilter(query, predicat);

                if (@object == null)
                    return null;

                return Converter.Convert<T>(@object);
            }
            catch (SequenceEmptyException)
            {
                return null;
            }
        }

        internal static object FirstOrDefaultWithFilterPrimitive(this IQueryable query, LambdaExpression condition)
        {
            try
            {
                return FirstWithFilterPrimitive(query, condition);
            }
            catch (SequenceEmptyException)
            {
                return null;
            }
        }

        internal static object FirstWithFilter(this IQueryable query)
        {
            try
            {
                var callExpression = Expression.Call(typeof (Queryable), "First", new[] {query.ElementType},
                    query.Expression);

                return query.Provider.Execute(callExpression);
            }
            catch (InvalidOperationException)
            {
                throw new SequenceEmptyException();
            }
        }

        internal static object FirstWithFilter<T>(this IQueryable query, Expression<Func<T, bool>> condition)
        {
//            var replaceCondition = new MemberTypeReplacement(query.ElementType).Visit(condition);

//            var callExpression = Expression.Call(typeof (Queryable), "First", new[] {query.ElementType},
//                query.Expression, replaceCondition);

//            return query.Provider.Execute(callExpression);

            return FirstWithFilterPrimitive(query, condition);
        }

        internal static object FirstWithFilterPrimitive(this IQueryable query, LambdaExpression condition)
        {
            try
            {
                var replaceCondition = new MemberTypeReplacement(query.ElementType).Visit(condition);

                var callExpression = Expression.Call(typeof(Queryable), "First", new[] { query.ElementType },
                    query.Expression, replaceCondition);

                return query.Provider.Execute(callExpression);
            }
            catch (InvalidOperationException)
            {
                throw new SequenceEmptyException();
            }
        }

        internal static IQueryable SelectWithFilter<TSource, TResult>(this IQueryable query, Expression<Func<TSource, TResult>> selector)
        {
            var replaceSelector = new MemberTypeReplacement(query.ElementType).Visit(selector);
            var resultType = typeof (TResult);
            if (resultType.Is<EntityBll>())
                resultType = Converter.BllTypeMap(resultType);

            var callExpression = Expression.Call(typeof(Queryable), "Select", new[] { query.ElementType, resultType }, query.Expression, replaceSelector);
            return query.Provider.CreateQuery(callExpression);
        }

        internal static int CountWithFilter(this IQueryable query)
        {
            var callExpression = Expression.Call(typeof(Queryable), "Count", new[] { query.ElementType }, query.Expression);
            return query.Provider.Execute<int>(callExpression);
        }

        internal static int CountWithFilter<TSource>(this IQueryable query,
            Expression<Func<TSource, bool>> predicate)
        {
            var replacePredicate = new MemberTypeReplacement(query.ElementType).Visit(predicate);
            var callExpression = Expression.Call(typeof(Queryable), "Count", new []{query.ElementType}, query.Expression, replacePredicate);

            return query.Provider.Execute<int>(callExpression);
        }

        internal static IQueryable<TResult> JoinWithFilter<TOuter, TInner, TKey, TResult>(this IQueryable outerQuery,
            IMapQueryable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            var replaceOuterKeySelector = new MemberTypeReplacement(outerQuery.ElementType).Visit(outerKeySelector);
            var replaceInnerKeySelector = new MemberTypeReplacement(inner.Query.ElementType).Visit(innerKeySelector);
            var replacesResultSelector = new MemberTypeReplacement(outerQuery.ElementType).Visit(resultSelector);
            replacesResultSelector = MemberTypeReplacement.ReplaceParameterType(inner.Query.ElementType,
                replacesResultSelector);

            var keyType = typeof (TKey).Is<EntityBll>() ? Converter.BllTypeMap(typeof (TKey)) : typeof (TKey);

            var callExpression = Expression.Call(typeof (Queryable), "Join",
                new[] {outerQuery.ElementType, inner.Query.ElementType, keyType, typeof (TResult)}, outerQuery.AsQueryable().Expression, inner.Query.Expression, replaceOuterKeySelector, replaceInnerKeySelector, replacesResultSelector);

            return outerQuery.Provider.CreateQuery<TResult>(callExpression);
        }

        internal static bool AnyWithFilter(this IQueryable query)
        {
            var callExpression = Expression.Call(typeof(Queryable), "Any", new[] { query.ElementType }, query.Expression);
            return query.Provider.Execute<bool>(callExpression);
        }

        internal static bool AnyWithFilter<TSourceBll>(this IQueryable query, Expression<Func<TSourceBll, bool>> predicate)
        {
            var replacePredicate = new MemberTypeReplacement(query.ElementType).Visit(predicate);
            var callExpression = Expression.Call(typeof(Queryable), "Any", new[] { query.ElementType }, query.Expression, replacePredicate);

            return query.Provider.Execute<bool>(callExpression);
        }

        internal static IEnumerable<T> ConvertTo<T>(IEnumerable query)
        {
            return (from object item in query select item).ToList().Select(ConvertElement<T>).OfType<T>().ToList();
        }

        internal static object ConvertElement<T>(object item)
        {
            if (item == null)
                return null;

            if (item.GetType() == typeof (T))
                return item;

//            return _container.Converter.Convert<T>((Entity) item);
            return Converter.Convert<EntityBll>((Entity) item);
        }
    }
}