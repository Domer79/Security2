using System;
using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Model
{
    /// <summary>
    /// Объект "Участник безопасности"
    /// </summary>
    public class Member : IMember
    {
        protected Member()
        {
            Roles = new HashSet<Role>();
        }

        /// <summary>
        /// Дополнительный идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Иденификатор участника в БД
        /// </summary>
        public int IdMember { get; set; }

        /// <summary>
        /// Имя участника
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список его ролей
        /// </summary>
        public HashSet<Role> Roles { get; set; }

        /// <summary>
        /// Список его ролей
        /// </summary>
        IList<IRole> IMember.Roles => new List<IRole>(Roles);
    }
}
