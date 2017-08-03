using System;
using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    /// <summary>
    /// Профиль пользователя
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Дополнительный идентификатор пользователя
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        int IdMember { get; }

        /// <summary>
        /// Логин
        /// </summary>
        string Login { get; }

        /// <summary>
        /// Имя
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        string MiddleName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Статус - Активен/Заблокироан
        /// </summary>
        bool Status { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        DateTime DateCreated { get; set; }

        /// <summary>
        /// Дата последней активности пользователя
        /// </summary>
        DateTime? LastActivityDate { get; set; }

        /// <summary>
        /// Список групп пользователя
        /// </summary>
        IList<IGroup> Groups { get; }
    }
}