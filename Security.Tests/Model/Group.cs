using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Tests.Model
{
    public class Group : IGroup
    {
        public Group()
        {
            Users = new HashSet<User>();
        }

        public int IdMember { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public HashSet<User> Users { get; set; }

        IList<IUser> IGroup.Users
        {
            get
            {
                return new List<IUser>(Users);
            }
        }
    }
}
