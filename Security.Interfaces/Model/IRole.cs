using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    /// <summary>
    /// Объект "Роль"
    /// </summary>
    public interface IRole
    {
        /// <summary>
        /// Идентификатор роли в БД
        /// </summary>
        int IdRole { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Описание
        /// </summary>
        string Description { get; }

        int IdApplication { get; set; }

        /// <summary>
        /// Список ее разрешений
        /// </summary>
        IList<IGrant> Grants { get; }

        /// <summary>
        /// Список участников, входящих в роль
        /// </summary>
        IList<IMember> Members { get; }

        /// <summary>
        /// Наименование приложения
        /// </summary>
        IApplication Application { get; set; }
    }
}