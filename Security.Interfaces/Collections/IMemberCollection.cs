using System;
using System.Collections.Generic;
using System.Linq;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    public interface IMemberCollection : IQueryable<IMember>, IDisposable
    {
        
    }
}