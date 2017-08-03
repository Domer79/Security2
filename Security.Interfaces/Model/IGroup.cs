using System;
using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    /// <summary>
    /// Объект Группа пользователей
    /// </summary>
    public interface IGroup
    {
        /// <summary>
        /// Дополнительный идентификатор пользователя
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Идентифкатор группы
        /// </summary>
        int IdMember { get; }

        /// <summary>
        /// Наименование группы
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Описание группы
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Список пользователей
        /// </summary>
        IList<IUser> Users { get; }
    }
}