using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Interfaces.Model;

namespace Security.Model
{
    public class Application : IApplication
    {
        /// <summary>
        /// Идентификатор приложения
        /// </summary>
        public int IdApplication { get; set; }

        /// <summary>
        /// Имя приложения. Является первичным ключов в БД
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Список ролей приложения
        /// </summary>
        public HashSet<Role> Roles { get; set; }

        /// <summary>
        /// Список объектов безопасности приложения
        /// </summary>
        public HashSet<SecObject> SecObjects { get; set; }

        /// <summary>
        /// Список типов доступа приложения
        /// </summary>
        public HashSet<AccessType> AccessTypes { get; set; }

        /// <summary>
        /// Список ролей приложения
        /// </summary>
        IList<IRole> IApplication.Roles => new List<IRole>(Roles);

        /// <summary>
        /// Список объектов безопасности приложения
        /// </summary>
        IList<ISecObject> IApplication.SecObjects => new List<ISecObject>(SecObjects);

        /// <summary>
        /// Список типов доступа приложения
        /// </summary>
        IList<IAccessType> IApplication.AccessTypes => new List<IAccessType>(AccessTypes);
    }
}
