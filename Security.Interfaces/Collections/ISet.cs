using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Security.Interfaces.Collections
{
    public interface ISet<T> : IEnumerable<T>, ISet
    {
        
    }

    public interface ISet : IEnumerable
    {
        IQueryable Query { get; set; }
    }
}