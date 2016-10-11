using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    public interface IMember
    {
        int IdMember { get; }
        string Name { get; }
        IList<IRole> Roles { get; }
    }
}