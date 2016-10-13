using System.Collections.Generic;
using System.Linq;
using Security.Interfaces.Model;
using Security.Tests.Tests;

namespace Security.Tests.Model
{
    public class Member : IMember
    {
        public int IdMember { get; set; }

        public string Name { get; set; }

        public HashSet<Role> Roles
        {
            get
            {
                var roles = Data.MemberRoles[this];
                return new HashSet<Role>((IEnumerable<Role>) roles);
            }
            set { }
        }

        IList<IRole> IMember.Roles
        {
            get
            {
                return new List<IRole>(Roles);
            }
        }
    }
}
