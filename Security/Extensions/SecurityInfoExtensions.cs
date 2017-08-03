using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Security.Exceptions;
using Security.Interfaces;

namespace Security.Extensions
{
    /// <summary>
    /// Добавляет методы расширения для объекта и сборки, которые возвращают информацию о сборке, если она поддерживает модель безопасности
    /// </summary>
    public static class SecurityInfoExtensions
    {
        /// <summary>
        /// Возвращает информацию о сборке <see cref="ISecurityApplicationInfo"/>
        /// </summary>
        /// <param name="anyObject"></param>
        /// <returns></returns>
        /// <exception cref="SecurityNotSupportedException"></exception>
        public static ISecurityApplicationInfo GetSecurityInfo(this object anyObject)
        {
            if (anyObject == null)
                throw new ArgumentNullException(nameof(anyObject));

            var securityAttribute = anyObject.GetType().Assembly.GetCustomAttribute<AssemblySecurityApplicationInfoAttribute>();
            if (securityAttribute == null)
                throw new SecurityNotSupportedException($"Сборка \"{anyObject.GetType().Assembly}\" не поддерживает модель безопасности");

            return securityAttribute;
        }

        /// <summary>
        /// Возвращает информацию о сборке <see cref="ISecurityApplicationInfo"/>
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        /// <exception cref="SecurityNotSupportedException"></exception>
        public static ISecurityApplicationInfo GetSecurityInfoFromAssembly(this Assembly assembly)
        {
            var assemblyString = assembly.ToString();
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            var securityAttribute = assembly.GetCustomAttribute<AssemblySecurityApplicationInfoAttribute>();
            if (securityAttribute == null)
                throw new SecurityNotSupportedException($"Сборка \"{assemblyString}\" не поддерживает модель безопасности");

            return securityAttribute;
        }
    }
}
