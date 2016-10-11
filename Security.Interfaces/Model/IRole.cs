using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    public interface IRole
    {
        int IdRole { get; }
        string Name { get; }
        string Description { get; }
        IList<IGrant> Grants { get; }
        IList<IMember> Members { get; }
    }
}