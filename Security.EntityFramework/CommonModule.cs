using Ninject.Modules;
using Ninject.Web.Common;
using Security.EntityDal;
using Security.Interfaces;

namespace Security.EntityFramework
{
    public class CommonModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<ISecurityFactory>().To<SecurityFactory>().InRequestScope();
            Bind<SecurityContext>().ToSelf();
        }
    }
}
