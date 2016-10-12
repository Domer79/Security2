using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itis.Common.Extensions;
using Ninject;
using Ninject.Modules;
using Security.Exceptions;
using Security.Interfaces.Collections;
using Security.Model.Entities;

namespace Security.Configurations
{
    public class Config
    {
        private static readonly StandardKernel _kernel = new StandardKernel();

        public static void RegisterAccessTypes(Type accessType)
        {
            if (!accessType.Is<Enum>())
                throw new InvalidOperationException("The accessType parameter must be Enumerator");

            RegisterAccessTypes(Enum.GetNames(accessType));
        }

        /// <summary>
        /// Регистрация типов доступа.
        /// </summary>
        /// <param name="accessTypes"></param>
        /// <exception cref="InvalidOperationException">Возникает, если присутствуют какие-либо выделенные разрешения</exception>
        public static void RegisterAccessTypes(string[] accessTypes)
        {
            var grantCollection = Get<IGrantCollection>();

            if (grantCollection.Count > 0)
                throw new CannotModifyAccessTypeException("You cannot modify accessType because there is data in Grants");

            var accessTypeCollection = Get<IAccessTypeCollection>();
            accessTypeCollection.Clear();
            foreach (var accessType in accessTypes.Select(a => new AccessType() {Name = a}))
            {
                accessTypeCollection.Add(accessType);
            }

            accessTypeCollection.SaveChanges();
        }

        internal static void RegisterCommonModule<TModule>() where TModule : INinjectModule, new()
        {
            if (!_kernel.HasModule(typeof(TModule).FullName))
                _kernel.Load<TModule>();
        }

        internal static T Get<T>()
        {
            return _kernel.Get<T>();
        }
    }
}
