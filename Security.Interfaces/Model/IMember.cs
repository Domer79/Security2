using System;
using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    /// <summary>
    /// Объект "Участник безопасности"
    /// </summary>
    public interface IMember
    {
        /// <summary>
        /// Дополнительный идентификатор пользователя
        /// </summary>
        Guid Id { get; set; }        
        
        /// <summary>
        /// Иденификатор участника в БД
        /// </summary>
        int IdMember { get; }

        /// <summary>
        /// Имя участника
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Список его ролей
        /// </summary>
        IList<IRole> Roles { get; }
    }
}