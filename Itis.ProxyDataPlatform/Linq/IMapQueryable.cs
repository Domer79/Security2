using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Itis.ProxyDataPlatform.Linq
{
    public interface IMapQueryable : IEnumerable
    {
        IQueryable Query { get; set; }
    }

    public interface IMapQueryable<out T> : IEnumerable<T>, IMapQueryable
    {
        IEnumerable<T> GetData();
//        T Execute();
//        T Execute(Expression expression);
    }

    public interface IMapOrderedQueryable<out T> : IMapQueryable<T>
    {
        
    }
}