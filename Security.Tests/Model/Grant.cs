using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Tests.Model
{
    public class Grant : IGrant
    {
        public int IdSecObject { get; set; }

        public int IdRole { get; set; }

        public int IdAccessType { get; set; }

        public Role Role { get; set; }
        public AccessType AccessType { get; set; }
        public SecObject SecObject { get; set; }

        IRole IGrant.Role
        {
            get { return Role; }
        }

        IAccessType IGrant.AccessType
        {
            get { return AccessType; }
        }

        ISecObject IGrant.SecObject
        {
            get { return SecObject; }
        }

        public static IEnumerable<IGrant> FakeCollection { get; set; } = new List<IGrant>();
    }
}
