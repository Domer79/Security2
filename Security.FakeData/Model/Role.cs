using System.Collections.Generic;
using System.Linq;
using Security.Interfaces.Model;

namespace Security.FakeData.Model
{
    public class Role : IRole
    {
        public int IdRole { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public HashSet<Grant> Grants
        {
            get
            {
                var grants = Data.GrantCollection.Where(g => g.Role == this);
                return new HashSet<Grant>((IEnumerable<Grant>)grants);
            }
            set { }
        }

        public HashSet<Member> Members
        {
            get
            {
                var members = Data.MemberRoles[this];
                return new HashSet<Member>((IEnumerable<Member>) members);
            }
            set { }
        }

        IList<IGrant> IRole.Grants
        {
            get
            {
                return new List<IGrant>(Grants);
            }
        }

        IList<IMember> IRole.Members
        {
            get
            {
                return new List<IMember>(Members);
            }
        }
    }
}
