using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    public interface IGroup
    {
        int IdMember { get; }
        string Name { get; }
        string Description { get; }
        IList<IUser> Users { get; }
    }
}