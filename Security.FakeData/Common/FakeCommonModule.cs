using Ninject.Modules;
using Security.FakeData.Collections;
using Security.FakeData.Model;
using Security.Interfaces;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;

namespace Security.FakeData.Common
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
            Bind<IGroupCollection>().To<GroupCollection>();
            Bind<IMemberCollection>().To<MemberCollection>();
            Bind<IRoleCollection>().To<RoleCollection>();
            Bind<ISecObjectCollection>().To<SecObjectCollection>();
            Bind<IUserCollection>().To<UserCollection>();
            Bind<ISecurityTools>().To<Tools>();
            Bind<IAccessType>().To<AccessType>();
        }
    }
}
