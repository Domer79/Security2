using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Tests.Model
{
    public class Member : IMember
    {
        protected Member()
        {
            Roles = new HashSet<Role>();
        }

        public int IdMember { get; set; }

        public string Name { get; set; }

        public HashSet<Role> Roles { get; set; }

        IList<IRole> IMember.Roles
        {
            get
            {
                return new List<IRole>(Roles);
            }
        }
    }
}
