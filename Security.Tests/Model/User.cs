using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Tests.Model
{
    public class User : IUser
    {
        public User()
        {
            Groups = new HashSet<Group>();
        }

        public int IdMember { get; set; }

        public string Login { get; set; }

        public byte[] Password { get; set; }

        public HashSet<Group> Groups { get; set; }

        IList<IGroup> IUser.Groups
        {
            get
            {
                return new List<IGroup>(Groups);
            }
        }
    }
}
