using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Security.Configurations;
using Security.Interfaces;
using Tools.Extensions;

namespace Security.Web
{

    internal abstract class BaseExecSecurityObjects<TAuthorizeAttribute> : ISecurityObjects where TAuthorizeAttribute: Attribute
    {
        private readonly List<Assembly> _assemblies = new List<Assembly>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected BaseExecSecurityObjects(IEnumerable<Assembly> assemblies)
        {
            _assemblies.AddRange(assemblies);
        }

        protected abstract Func<MethodInfo, bool> GetActionFilter();
        protected abstract string GetInterfaceName();

        public IEnumerator<ISecurityObject> GetEnumerator()
        {
            return GetSecurityObjects().GetEnumerator();
        }

        private IEnumerable<ISecurityObject> GetSecurityObjects()
        {
            var securityObjects = new List<ISecurityObject>();
            securityObjects.AddRange(GetSecurityObjectsFromAuthorizeAttributes());

            return securityObjects;
        }

        /// <summary>
        /// Поиск объектов безопасности в атрибутах
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ISecurityObject> GetSecurityObjectsFromAuthorizeAttributes()
        {
            var securityObjects = new List<ISecurityObject>();

            foreach (var assembly in _assemblies)
            {
                securityObjects.AddRange(GetSecurityObjectsFromAssembly(assembly));
            }

            return securityObjects;
        }

        /// <summary>
        /// Поиск контроллеров и действий, для которых установлена проверка авторизации.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private IEnumerable<ISecurityObject> GetSecurityObjectsFromAssembly(Assembly assembly)
        {
            var securityObjects = new List<ISecurityObject>();
            var controllerTypes = assembly.GetExportedTypes().Where(t => t.GetInterface(GetInterfaceName()) != null);

            foreach (var controllerType in controllerTypes)
            {
                if (controllerType.IsDefined(typeof(TAuthorizeAttribute)))
                {
                    securityObjects.AddRange(GetSecurityObjectsFromAuthorizeController(controllerType));
                    continue;
                }

                securityObjects.AddRange(GetSecurityObjectsFromAuthorizeActions(controllerType));
            }

            return securityObjects;
        }

        /// <summary>
        /// Поиск действий контроллера, для которого установлен атрибут <see cref="AuthorizeAttribute"/>
        /// </summary>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        private IEnumerable<ISecurityObject> GetSecurityObjectsFromAuthorizeController(Type controllerType)
        {
            var securityObjects = new List<ISecurityObject>();
            var methods = controllerType.GetMethods().Where(GetActionFilter());
            foreach (var methodInfo in methods)
            {
                var anonymousAttribute = (AllowAnonymousAttribute)methodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute));

                //Если для действия установлен атрибут AllowAnonymousAttribute, переходим к следующей итерации цикла
                if (anonymousAttribute != null)
                    continue;

                var authorizeAttribute = (TAuthorizeAttribute)methodInfo.GetCustomAttribute(typeof(TAuthorizeAttribute));
                //Если атрибут отсутствует создаем объект безопасности
                if (authorizeAttribute == null)
                {
                    ISecurityObject securityObject = new ExecSecurityObject()
                    {
                        ObjectName = Mvc.AuthorizeAttribute.GetObjectName(controllerType.Name, methodInfo.Name)
                    };
                    securityObjects.Add(securityObject);
                    continue;
                }

                ((ISecurityObject)authorizeAttribute).ObjectName = ((ISecurityObject)authorizeAttribute).ObjectName ??
                                                                   Mvc.AuthorizeAttribute.GetObjectName(controllerType.Name, methodInfo.Name);

                //Если атрибут есть и указано имя объекта просто добавляем его в список
                securityObjects.Add((ISecurityObject) authorizeAttribute);
            }

            return securityObjects;

        }

        /// <summary>
        /// Поиск действий контроллера, для которых установлен атрибут <see cref="AuthorizeAttribute"/>
        /// </summary>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        private static IEnumerable<ISecurityObject> GetSecurityObjectsFromAuthorizeActions(Type controllerType)
        {
            var securityObjects = new List<ISecurityObject>();
            var methods = controllerType.GetMethods()/*.Where(m => m.ReturnType.Is<ActionResult>())*/.Where(m => m.IsDefined(typeof(TAuthorizeAttribute)));
            foreach (var methodInfo in methods)
            {
                var authorizeAttribute = (TAuthorizeAttribute)methodInfo.GetCustomAttribute(typeof(TAuthorizeAttribute));

                if (((ISecurityObject)authorizeAttribute).ObjectName == null)
                {
                    var securityObject = new ExecSecurityObject() { ObjectName = Mvc.AuthorizeAttribute.GetObjectName(controllerType.Name, methodInfo.Name) };
                    securityObjects.Add(securityObject);
                    continue;
                }

                securityObjects.Add((ISecurityObject) authorizeAttribute);
            }

            return securityObjects;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class ExecSecurityObject : ISecurityObject
        {
            private string _accessType = Config.Exec;

            public string ObjectName { get; set; }

            public string AccessType
            {
                get { return _accessType; }
                set { _accessType = Config.Exec; }
            }
        }
    }

    /// <summary>
    /// Осуществляет поиск контроллеров и его действий, помеченных атрибутом <see cref="Security.Web.Mvc.AuthorizeAttribute"/> и преобразует эти атрибуты в объекты <see cref="ISecurityObject"/>, для последующей передачи в метод <see cref="Config.RegisterApplication(Security.Interfaces.ISecurityObjects[])"/>
    /// </summary>
    /// <code>
    /// Config.RegisterApplication(new ExecSecurityObjects(new []{Assembly.GetExecutingAssembly()}));
    /// </code>
    internal class ExecMvcSecurityObjects : BaseExecSecurityObjects<Mvc.AuthorizeAttribute>
    {
        public ExecMvcSecurityObjects(IEnumerable<Assembly> assemblies) 
            : base(assemblies)
        {
        }

        protected override Func<MethodInfo, bool> GetActionFilter()
        {
            return m => m.ReturnType.Is<ActionResult>();
        }

        protected override string GetInterfaceName()
        {
            return "IController";
        }
    }

    /// <summary>
    /// Осуществляет поиск контроллеров и его действий, помеченных атрибутом <see cref="Security.Web.Http.AuthorizeAttribute"/> и преобразует эти атрибуты в объекты <see cref="ISecurityObject"/>, для последующей передачи в метод <see cref="Config.RegisterApplication(Security.Interfaces.ISecurityObjects[])"/>
    /// </summary>
    /// <code>
    /// Config.RegisterApplication(new ExecSecurityObjects(new []{Assembly.GetExecutingAssembly()}));
    /// </code>
    internal class ExecHttpSecurityObjects : BaseExecSecurityObjects<Http.AuthorizeAttribute>
    {
        public ExecHttpSecurityObjects(IEnumerable<Assembly> assemblies) 
            : base(assemblies)
        {
        }

        protected override Func<MethodInfo, bool> GetActionFilter()
        {
            return m => m.IsPublic;
        }

        protected override string GetInterfaceName()
        {
            return "IHttpController";
        }
    }

    public class ExecSecurityObjects: ISecurityObjects
    {
        private readonly List<ISecurityObject> _securityObjects = new List<ISecurityObject>();

        public ExecSecurityObjects(IEnumerable<Assembly> assemblies)
        {
            var assemblyArray = assemblies as Assembly[] ?? assemblies.ToArray();

            _securityObjects.AddRange(new ExecMvcSecurityObjects(assemblyArray));
            _securityObjects.AddRange(new ExecHttpSecurityObjects(assemblyArray));
        }

        public IEnumerator<ISecurityObject> GetEnumerator()
        {
            return _securityObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}