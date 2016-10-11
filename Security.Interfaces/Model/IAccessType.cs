using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    public interface IAccessType
    {
        int IdAccessType { get; }
        string Name { get; }
        IList<IGrant> Grants { get; }
    }
}