using System;
using System.Linq;
using Ninject;
using Ninject.Modules;
using Security.Exceptions;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;
using Tools.Extensions;

namespace Security.Configurations
{
    public class Config
    {
        private static readonly StandardKernel Kernel = new StandardKernel();

        internal static Type AccessType { get; private set; }

        /// <summary>
        /// Регистрация типов доступа.
        /// </summary>
        /// <param name="accessType"></param>
        /// <exception cref="InvalidOperationException">Возникает, если присутствуют какие-либо выделенные разрешения</exception>
        public static void RegisterAccessTypes(Type accessType)
        {
            if (!accessType.Is<Enum>())
                throw new InvalidOperationException("The accessType parameter must be Enumerator");

            AccessType = accessType;
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
            foreach (var accessType in accessTypes.Select(GetAccessType))
            {
                accessTypeCollection.Add(accessType);
            }

            accessTypeCollection.SaveChanges();
        }

        internal static void RegisterCommonModule<TModule>() where TModule : INinjectModule, new()
        {
            if (!Kernel.HasModule(typeof(TModule).FullName))
                Kernel.Load<TModule>();
        }

        internal static T Get<T>()
        {
            return Kernel.Get<T>();
        }

        #region Helpers

        private static IAccessType GetAccessType(string a)
        {
            var accessType = Get<IAccessType>();
            accessType.Name = a;
            return accessType;
        }

        #endregion

    }
}
