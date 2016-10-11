using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Security.Linq
{
    public static class Set
    {
        public static ISet<T> Where<T>(this ISet<T> setQuery, Expression<Func<T, bool>> predicat) where T : class
        {
            
        }
    }
}
