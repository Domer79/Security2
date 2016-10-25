using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.FakeData.Model
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
                return new HashSet<Role>(roles);
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
