using System;
using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Model
{
    /// <summary>
    /// Объект "Роль"
    /// </summary>
    public class Role : IRole
    {
        public Role()
        {
            Grants = new HashSet<Grant>();
        }

        /// <summary>
        /// Идентификатор роли в БД
        /// </summary>
        public int IdRole { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        public int IdApplication { get; set; }

        /// <summary>
        /// Список ее разрешений
        /// </summary>
        public HashSet<Grant> Grants { get; set; }

        /// <summary>
        /// Список участников, входящих в роль
        /// </summary>
        public HashSet<Member> Members { get; set; }

        /// <summary>
        /// Наименование приложения
        /// </summary>
        public Application Application { get; set; }

        /// <summary>
        /// Список ее разрешений
        /// </summary>
        IList<IGrant> IRole.Grants => new List<IGrant>(Grants);

        /// <summary>
        /// Список участников, входящих в роль
        /// </summary>
        IList<IMember> IRole.Members => new List<IMember>(Members);

        /// <summary>
        /// Наименование приложения
        /// </summary>
        IApplication IRole.Application
        {
            get { return Application; }
            set { Application = (Application) value; }
        }
    }
}
