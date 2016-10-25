using Security.Interfaces.Model;

namespace Security.FakeData.Model
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
            set { Role = (Role) value; }
        }

        IAccessType IGrant.AccessType
        {
            get { return AccessType; }
            set { AccessType = (AccessType) value; }
        }

        ISecObject IGrant.SecObject
        {
            get { return SecObject; }
            set { SecObject = (SecObject) value; }
        }
    }
}
