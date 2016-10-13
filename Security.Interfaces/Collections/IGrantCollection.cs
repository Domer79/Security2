using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    public interface IGrantCollection : ICollection<IGrant>, ISavedCollection
    {
        void Add(IRole role, ISecObject secObject, IAccessType accessType);
        bool Remove(IRole role, ISecObject secObject, IAccessType accessType);
    }
}