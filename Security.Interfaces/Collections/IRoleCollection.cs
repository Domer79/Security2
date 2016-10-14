using System.Collections.Generic;
using Security.Interfaces.Base;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    public interface IRoleCollection : IQueryableCollection<IRole>, ISavedCollection
    {
        
    }
}