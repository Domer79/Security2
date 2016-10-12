using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Security.Interfaces.Collections;

namespace Security.Infrastructure
{
    public class CommonModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IAccessTypeCollection>().To<AccessTypeCollection>();
            Bind<IGrantCollection>().To<GrantCollection>();
            Bind<IGroupCollection>().To<GroupCollection>();
            Bind<IMemberCollection>().To<MemberCollection>();
            Bind<IRoleCollection>().To<RoleCollection>();
            Bind<ISecObjectCollection>().To<SecObjectCollection>();
            Bind<IUserCollection>().To<UserCollection>();
        }
    }
}
