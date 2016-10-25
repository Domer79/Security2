using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.FakeData.Model
{
    public class Group : IGroup, IMember
    {
        private readonly Member _member;

        public Group()
        {
            _member = new Member();
            Data.MemberCollection.Add(_member);
        }

        public int IdMember { get; set; }

        public string Name
        {
            get { return _member.Name; }
            set { _member.Name = value; }
        }

        public IList<IRole> Roles => ((IMember)_member).Roles;

        public string Description { get; set; }

        public HashSet<User> Users
        {
            get
            {
                var users = Data.UserGroups[this];
                return new HashSet<User>((IEnumerable<User>) users);
            }
            set { }
        }

        IList<IUser> IGroup.Users
        {
            get
            {
                return new List<IUser>(Users);
            }
        }
    }
}
