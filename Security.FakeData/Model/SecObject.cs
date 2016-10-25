using System.Collections.Generic;
using System.Linq;
using Security.Interfaces.Model;

namespace Security.FakeData.Model
{
    public class SecObject : ISecObject
    {
        public int IdSecObject { get; set; }

        public string ObjectName { get; set; }

        public HashSet<Grant> Grants
        {
            get
            {
                var grants = Data.GrantCollection.Where(g => g.SecObject == this);
                return new HashSet<Grant>((IEnumerable<Grant>) grants);
            }

            set { }
        }

        IList<IGrant> ISecObject.Grants
        {
            get
            {
                return new List<IGrant>(Grants);
            }
        }

        /// <summary>
        /// ���������� ������, ������� ������������ ������� ������.
        /// </summary>
        /// <returns>
        /// ������, �������������� ������� ������.
        /// </returns>
        public sealed override string ToString()
        {
            return ObjectName;
        }
    }
}
