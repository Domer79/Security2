using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Security.Interfaces.Model
{
    /// <summary>
    /// Приложение
    /// </summary>
    public interface IApplication
    {
        int IdApplication { get; set; }

        /// <summary>
        /// Имя приложения. Является первичным ключов в БД
        /// </summary>
        string AppName { get; set; } 

        /// <summary>
        /// Описание
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Список ролей приложения
        /// </summary>
        IList<IRole> Roles { get; }

        /// <summary>
        /// Список объектов безопасности приложения
        /// </summary>
        IList<ISecObject> SecObjects { get; }

        /// <summary>
        /// Список типов доступа приложения
        /// </summary>
        IList<IAccessType> AccessTypes { get; }
    }
}