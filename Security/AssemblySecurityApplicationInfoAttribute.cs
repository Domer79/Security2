using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Interfaces;
using System.Diagnostics;

namespace Security
{
    /// <summary>
    /// Помечает сборку для решистрации ее в Системе управления доступом и поддерживающую модель безопасности
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    [DebuggerDisplay("ApplicationName = {ApplicationName}; Description = {Description}")]
    public class AssemblySecurityApplicationInfoAttribute: Attribute, ISecurityApplicationInfo
    {
        public AssemblySecurityApplicationInfoAttribute(string applicationName, string description)
        {
            ApplicationName = applicationName;
            Description = description;
        }

        /// <summary>
        /// Имя приложения
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Описание приложения
        /// </summary>
        public string Description { get; set; }
    }
}
