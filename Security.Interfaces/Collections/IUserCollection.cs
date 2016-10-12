using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    public interface IUserCollection : ICollection<IUser>, ISavedCollection
    {
    }
}
