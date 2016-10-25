using System;
using System.Collections.Generic;
using System.Linq;

namespace Security.Interfaces.Base
{
    public interface IQueryableCollection<T> : ICollection<T>, IQueryable<T>, IDisposable, ISavedCollection
    {

    }
}