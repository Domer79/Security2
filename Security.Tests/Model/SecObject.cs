using System.Collections.Generic;
using System.Linq;
using Security.Interfaces.Model;
using Security.Tests.Tests;

namespace Security.Tests.Model
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
        /// Возвращает строку, которая представляет текущий объект.
        /// </summary>
        /// <returns>
        /// Строка, представляющая текущий объект.
        /// </returns>
        public sealed override string ToString()
        {
            return ObjectName;
        }
    }
}
