using System.Collections.Generic;
using Security.Interfaces.Base;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    public interface IGrantCollection : IQueryableCollection<IGrant>
    {
        void Add(string roleName, string secObjectName, string accessTypeName);
        bool Remove(string roleName, string secObjectName, string accessTypeName);
    }
}