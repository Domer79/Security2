using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    public interface IRoleCollection : ICollection<IRole>, ISavedCollection
    {
        
    }
}