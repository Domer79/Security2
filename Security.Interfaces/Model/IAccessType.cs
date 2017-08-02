using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    /// <summary>
    /// Тип доступа
    /// </summary>
    public interface IAccessType
    {
        /// <summary>
        /// Идентификатор типа доступа в БД
        /// </summary>
        int IdAccessType { get; set; }

        /// <summary>
        /// Наименование типа доступа, сопоставляется с наименованием перечислителя в клиентском ПО
        /// </summary>
        string Name { get; set; }

        int IdApplication { get; set; }

        /// <summary>
        /// Список разрешений
        /// </summary>
        IList<IGrant> Grants { get; }

        /// <summary>
        /// Привязанные к типу доступа объекты безопасности
        /// </summary>
        IList<ISecObject> SecObjects { get; }

        /// <summary>
        /// Приложение
        /// </summary>
        IApplication Application { get; set; }
    }
}