using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Interfaces.Collections;

namespace Security.Interfaces
{
    internal interface ISecurityFactory
    {
        IAccessTypeCollection CreateAccessTypeCollection();

        IGrantCollection CreateGrantCollection();

        IGroupCollection CreateGroupCollection();

        IMemberCollection CreateMemberCollection();

        IRoleCollection CreateRoleCollection();

        ISecObjectCollection CreateSecObjectCollection();

        IUserCollection CreateUserCollection();
    }
}
