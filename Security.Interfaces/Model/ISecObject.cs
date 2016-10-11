using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    public interface ISecObject
    {
        int IdSecObject { get; }
        string ObjectName { get; }
        IList<IGrant> Grants { get; }
    }
}