using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    public interface IGrantCollection : ICollection<IGrant>, ISavedCollection
    {
        
    }
}