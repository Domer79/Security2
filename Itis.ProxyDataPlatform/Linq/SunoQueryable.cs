using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Itis.ProxyDataPlatform.Base;
using Itis.ProxyDataPlatform.Converting;
using Itis.ProxyDataPlatform.DatabaseContext;

namespace Itis.ProxyDataPlatform.Linq
{
    public class SunoQueryable<T> : IMapQueryable<T>, IMapOrderedQueryable<T>
    {
//        private readonly BaseContainer _container;

        public SunoQueryable(IQueryable query)
        {
            ((IMapQueryable<T>) this).Query = query;
        }

//        public SunoQueryable(BaseContainer container, Type type)
//            : this(container.Set(container.Converter.BllTypeMap(type)))
//        {
//            _container = container;
//        }

        IQueryable IMapQueryable.Query { get; set; }

        IEnumerable<T> IMapQueryable<T>.GetData()
        {
            var type = typeof (T);
            if (!type.IsValueType)
                return MapQueryableHelper.ConvertTo<T>(((IMapQueryable<T>) this).Query);
            return (IEnumerable<T>) ((IMapQueryable<T>)this).Query;
        }

//        public T Execute()
//        {
//            return (T) MapQueryableHelper.ConvertElement<T>(((IMapQueryable<T>)this).Query.Provider.Execute(((IMapQueryable<T>)this).Query.Expression));
//        }
//
//        public T Execute(Expression expression)
//        {
//            return (T) MapQueryableHelper.ConvertElement<T>(((IMapQueryable<T>) this).Query.Provider.Execute(expression));
//        }

        public IEnumerator<T> GetEnumerator()
        {
//            _container.Converter.EntityStack.Push();
            try
            {
                return ((IMapQueryable<T>)this).GetData().GetEnumerator();
            }
            finally
            {
//                _container.Converter.EntityStack.Pop();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Возвращает строку, которая представляет текущий объект.
        /// </summary>
        /// <returns>
        /// Строка, представляющая текущий объект.
        /// </returns>
        public override string ToString()
        {
            return ((IMapQueryable) this).Query.ToString();
        }
    }

    public class Firstable<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Firstable(IQueryable query)
        {
            Query = query;
        }

        internal IQueryable Query { get; set; }

        public T Get()
        {
            return Converter.Convert<T>(Query.FirstWithFilter());
        }

        public T GetOrDefault()
        {
            var entity = Query.FirstOrDefaultWithFilter();
            if (entity == null)
                return null;

            return Converter.Convert<T>(entity);
        }
    }
}
