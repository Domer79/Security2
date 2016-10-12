using Ninject.Modules;
using Security.Interfaces.Collections;
using Security.Tests.Collections;

namespace Security.Tests.Common
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
