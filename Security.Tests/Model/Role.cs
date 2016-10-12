using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Tests.Model
{
    public class Role : IRole
    {
        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="T:System.Object"/>.
        /// </summary>
        public Role()
        {
            Grants = new HashSet<Grant>();
        }

        public int IdRole { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public HashSet<Grant> Grants { get; set; }

        public HashSet<Member> Members { get; set; }

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
