using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject;
using Ninject.Modules;
using Ninject.Parameters;
using Security.Exceptions;
using Security.Extensions;
using Security.Interfaces;
using Security.Interfaces.Model;
using Tools.Extensions;
using Security.Interfaces.Collections;

namespace Security.Configurations
{
    /// <summary>
    /// Предназначен для регистрации сборки, к которой будет применен механизм ограничения доступа
    /// </summary>
    public class Config
    {
        private static readonly StandardKernel Kernel = new StandardKernel();
        public const string Exec = "Exec";

        /// <summary>
        /// Регистрация типов доступа.
        /// </summary>
        /// <param name="accessType"></param>
        /// <param name="securityApplicationInfo"></param>
        /// <exception cref="InvalidOperationException">Возникает, если присутствуют какие-либо выделенные разрешения</exception>
        private static void RegisterAccessTypes(Type accessType, ISecurityApplicationInfo securityApplicationInfo)
        {
            var accessTypes = GetAccessTypesFromEnum(accessType);
            RegisterAccessTypes(accessTypes.Concat(new []{Exec}).ToArray(), securityApplicationInfo);
        }

        /// <summary>
        /// Регистрация типов доступа.
        /// </summary>
        /// <param name="accessTypes"></param>
        /// <param name="securityApplicationInfo"></param>
        /// <exception cref="InvalidOperationException">Возникает, если присутствуют какие-либо выделенные разрешения</exception>
        private static void RegisterAccessTypes(string[] accessTypes, ISecurityApplicationInfo securityApplicationInfo)
        {
            var securityFactory = Get<ISecurityFactory>();
            securityFactory.ApplicationName = securityApplicationInfo.ApplicationName;
            securityFactory.CreateAppIfNoExists(securityApplicationInfo);
            
            foreach (var accessName in accessTypes)
            {
                AddAccessType(accessName, securityFactory);
            }

            var accessTypeCollection = securityFactory.CreateAccessTypeCollection();
            var exceptAccessNames = accessTypeCollection.Select(at => at.Name).Except(accessTypes).ToList();
            foreach (var accessName in exceptAccessNames)
            {
                TryDeleteAccessName(accessName, securityFactory);
            }
        }

        /// <summary>
        /// Регистрация модуля Ninject
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        public static void RegisterCommonModule<TModule>() where TModule : INinjectModule, new()
        {
            if (!Kernel.HasModule(typeof (TModule).FullName))
            {
                Kernel.Load<TModule>();
            }
        }

        private static void AddAccessType(string accessName, ISecurityFactory securityFactory)
        {
            var accessTypeCollection = securityFactory.CreateAccessTypeCollection();
            if (string.IsNullOrEmpty(accessName))
                throw new AccessTypeValidException(accessName);

            if (accessTypeCollection.Any(at => at.Name == accessName && at.IdApplication == securityFactory.CurrentApplication.IdApplication))
                return;

            accessTypeCollection.Add(GetAccessType(accessName, securityFactory));
            accessTypeCollection.SaveChanges();
        }

        private static bool TryDeleteAccessName(string accessName, ISecurityFactory securityFactory)
        {
            var accessTypeCollection = securityFactory.CreateAccessTypeCollection();
            try
            {
                var accessType = accessTypeCollection.FirstOrDefault(at => at.Name == accessName);
                if (accessType == null)
                    throw new AccessTypeMissingException(accessName);

                accessTypeCollection.Remove(accessType);
                return accessTypeCollection.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        internal static T Get<T>(params IParameter[] parameters)
        {
            return Kernel.Get<T>(parameters);
        }

        #region Register Security Objects

        /// <summary>
        /// Возвращает список всех зарегистрированных объектов безопасности
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<ISecurityObject> GetSecurityObjects(ISecurityObjects[] securityObjectses)
        {
            var securityObjects = new List<ISecurityObject>();
            foreach (var sObjects in securityObjectses)
            {
                securityObjects.AddRange(sObjects);
            }

            return securityObjects;
        }

        private static void SetUpSecurityObjects(string applicationName, ISecurityObjects[] securityObjectses)
        {
            using (var security = new CoreSecurity(applicationName))
            {
                var securityObjects = GetSecurityObjects(securityObjectses).GroupBy(so => so.AccessType);
                foreach (var groupedSecurityObject in securityObjects)
                {
                    SetUpSecurityObjectsByAccessType(security, groupedSecurityObject.Key, groupedSecurityObject.Select(so => so.ObjectName));
                }
            }
        }

        private static void SetUpSecurityObjectsByAccessType(CoreSecurity security, string accessType, IEnumerable<string> securityObjects)
        {
            var idAccessType = security.GetAccessTypes()
                .Where(e => e.Name == accessType)
                .Select(e => e.IdAccessType)
                .Single();

            var sameInstalledObjects = security.SecObjectCollection
                .Where(e => securityObjects.Contains(e.ObjectName) && e.IdAccessType == idAccessType)
                .Select(e => e.ObjectName);

            var objects = securityObjects as string[] ?? securityObjects.ToArray();
            var newSecObjects = objects.Except(sameInstalledObjects, StringComparer.OrdinalIgnoreCase);

            foreach (var secObject in newSecObjects.Select(s => new SecObject() {ObjectName = s, IdAccessType = idAccessType}))
            {
                security.SecObjectCollection.Add(secObject);
            }

            security.SaveChanges();
            DeleteExceptSecObjects(objects, security.SecObjectCollection, idAccessType);
        }

        private class SecObject : ISecObject
        {
            public int IdSecObject { get; }
            public string ObjectName { get; set; }
            public int IdApplication { get; set; }
            public int IdAccessType { get; set; }
            public IList<IGrant> Grants { get; }
            public IApplication Application { get; set; }
            public IAccessType AccessType { get; set; }
        }

        #endregion

        #region Application Register

        /// <summary>
        /// Регистрация приложения с передачей списка его объектов безопасности. Наименование приложения ищется в сборке вызывающей данный метод
        /// </summary>
        /// <param name="securityObjectes">Список объектов безопасности</param>
        public static void RegisterApplication(params ISecurityObjects[] securityObjectes)
        {
            var securityAssembly = Assembly.GetCallingAssembly();
            RegisterApplication(securityAssembly, null, securityObjectes);
        }

        /// <summary>
        /// Регистрация приложения с передачей типов доступа, которые необходимо зарегистрировать и списка его объектов безопасности. Наименование приложения ищется в сборке вызывающей данный метод
        /// </summary>
        /// <param name="accessType">Типы доступа, должны быть определены как тип <see cref="Enum"/></param>
        /// <param name="securityObjectes">Список объектов безопасности</param>
        public static void RegisterApplication(Type accessType, params ISecurityObjects[] securityObjectes)
        {
            var securityAssembly = Assembly.GetCallingAssembly();
            RegisterApplication(securityAssembly, accessType, securityObjectes);
        }

        /// <summary>
        /// Регистрация приложения с передачей сборки и списка его объектов безопасности.
        /// </summary>
        /// <param name="securityAssembly">Сборка, помеченная атрибутом <see cref="AssemblySecurityApplicationInfoAttribute"/></param>
        /// <param name="securityObjectes">Список объектов безопасности</param>
        public static void RegisterApplication(Assembly securityAssembly, params ISecurityObjects[] securityObjectes)
        {
            RegisterApplication(securityAssembly, null, securityObjectes);
        }

        /// <summary>
        /// Регистрация приложения с передачей сборкиб, типов доступа, которые необходимо зарегистрировать и списка его объектов безопасности.
        /// </summary>
        /// <param name="securityAssembly">Сборка, помеченная атрибутом <see cref="AssemblySecurityApplicationInfoAttribute"/></param>
        /// <param name="accessType">Типы доступа, должны быть определены как тип <see cref="Enum"/></param>
        /// <param name="securityObjectes">Список объектов безопасности</param>
        /// <exception cref="AccessTypeMissingException">Возникает при синхронизации типов доступа, при удалении более не нужных типов, в случае их отсутствия</exception>
        /// <exception cref="AccessTypeValidException">Возникакет при попытке добавить тип доступа, значение строки которого является пустым или null</exception>
        public static void RegisterApplication(Assembly securityAssembly, Type accessType, params ISecurityObjects[] securityObjectes)
        {
            if (securityAssembly == null)
                throw new ArgumentNullException(nameof(securityAssembly));

            if (securityObjectes == null)
                throw new ArgumentNullException(nameof(securityObjectes));

            var securityAppInfo = securityAssembly.GetSecurityInfoFromAssembly();
            RegisterAccessTypes(accessType, securityAppInfo);
            SetUpSecurityObjects(securityAppInfo.ApplicationName, securityObjectes);
        }

        #endregion

        #region Helpers

        private static IAccessType GetAccessType(string a, ISecurityFactory securityFactory)
        {
            var accessType = securityFactory.GetAccessType();
            accessType.Name = a;
            return accessType;
        }

        private static string[] GetAccessTypesFromEnum(Type accessType)
        {
            if (accessType == null)
                return new string[] { };

            if (!accessType.Is<Enum>())
                throw new InvalidOperationException("The accessType parameter must be Enumerator");

            var accessTypes = Enum.GetNames(accessType);
            return accessTypes;
        }

        private static void DeleteExceptSecObjects(IEnumerable<string> securityObjects, ISecObjectCollection objectCollection, int idAccessType)
        {
            var secObjects = objectCollection.Where(e => !securityObjects.Contains(e.ObjectName) && e.IdAccessType == idAccessType);
            objectCollection.RemoveRange(secObjects);
            objectCollection.SaveChanges();
        }

        #endregion
    }
}
