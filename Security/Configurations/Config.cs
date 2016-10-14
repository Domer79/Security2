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
            SetNewAccessTypes(accessTypes);
        }

        public static void SetNewAccessTypes(string[] accessNames)
        {
            foreach (var accessName in accessNames)
            {
                AddAccessType(accessName);
            }

            var accessTypeCollection = Get<IAccessTypeCollection>();
            var exceptAccessNames = accessTypeCollection.Select(at => at.Name).Except(accessNames).ToList();
            foreach (var accessName in exceptAccessNames)
            {
                TryDeleteAccessName(accessName);
            }
        }

        private static void AddAccessType(string accessName)
        {
            var accessTypeCollection = Get<IAccessTypeCollection>();
            if (string.IsNullOrEmpty(accessName))
                throw new AccessTypeValidException(accessName);

            if (accessTypeCollection.Any(at => at.Name == accessName))
                return;

            accessTypeCollection.Add(GetAccessType(accessName));
            accessTypeCollection.SaveChanges();
        }

        private static void TryDeleteAccessName(string accessName)
        {
            var accessTypeCollection = Get<IAccessTypeCollection>();
            try
            {
                var accessType = accessTypeCollection.FirstOrDefault(at => at.Name == accessName);
                if (accessType == null)
                    throw new AccessTypeMissingException(accessName);

                accessTypeCollection.Remove(accessType);
                accessTypeCollection.SaveChanges();
            }
            catch (Exception e)
            {
                throw new AccessTypeDeleteException(e.Message, accessName);
            }
        }

        public static void RegisterCommonModule<TModule>() where TModule : INinjectModule, new()
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
