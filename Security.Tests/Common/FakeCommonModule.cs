using Ninject.Modules;
using Security.Interfaces;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;
using Security.Tests.Collections;
using Security.Tests.Model;

namespace Security.Tests.Common
{
    public class FakeCommonModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IAccessTypeCollection>().To<AccessTypeCollection>();
            Bind<IGrantCollection>().To<GrantCollection>();
            Bind<IGroupCollection>().To<Collections.GroupCollection>();
            Bind<IMemberCollection>().To<Collections.MemberCollection>();
            Bind<IRoleCollection>().To<Collections.RoleCollection>();
            Bind<ISecObjectCollection>().To<Collections.SecObjectCollection>();
            Bind<IUserCollection>().To<Collections.UserCollection>();
            Bind<ISecurityTools>().To<Tools>();
            Bind<IAccessType>().To<AccessType>();
        }
    }
}
