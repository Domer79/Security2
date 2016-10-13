using System.Collections.Generic;
using System.Linq;
using Security.Interfaces.Model;
using Security.Tests.Tests;

namespace Security.Tests.Model
{
    public class User : IUser, IMember
    {
        private readonly Member _member;

        public User()
        {
            _member = new Member();
            Data.MemberCollection.Add(_member);
        }

        public int IdMember { get; set; }

        public string Name => _member?.Name;

        public IList<IRole> Roles => ((IMember)_member).Roles;

        public string Login
        {
            get { return _member?.Name; }
            set { _member.Name = value; }
        }

        public byte[] Password { get; set; }

        public HashSet<Group> Groups
        {
            get
            {
                var users = Data.UserGroups[this];
                return new HashSet<Group>((IEnumerable<Group>)users);
            }
            set { }
        }

        IList<IGroup> IUser.Groups
        {
            get
            {
                return new List<IGroup>(Groups);
            }
        }
    }
}
