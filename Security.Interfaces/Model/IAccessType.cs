using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    public interface IAccessType
    {
        int IdAccessType { get; set; }
        string Name { get; set; }
        IList<IGrant> Grants { get; }
    }
}