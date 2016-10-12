using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Tests.Model
{
    public class AccessType : IAccessType
    {
        public AccessType()
        {
            Grants = new HashSet<Grant>();
        }

        public int IdAccessType { get; set; }

        public string Name { get; set; }

        public HashSet<Grant> Grants { get; set; }

        IList<IGrant> IAccessType.Grants
        {
            get { return new List<IGrant>(Grants); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
