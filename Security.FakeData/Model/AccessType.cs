using System.Collections.Generic;
using System.Linq;
using Security.Interfaces.Model;

namespace Security.FakeData.Model
{
    public class AccessType : IAccessType
    {
        public int IdAccessType { get; set; }

        public string Name { get; set; }

        public HashSet<Grant> Grants
        {
            get
            {
                var grants = Data.GrantCollection.Where(g => g.AccessType == this);
                return new HashSet<Grant>((IEnumerable<Grant>) grants);
            }
            set { }
        }

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
