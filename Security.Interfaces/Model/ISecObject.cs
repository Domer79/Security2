using System.Collections.Generic;

namespace Security.Interfaces.Model
{
    /// <summary>
    /// Объект безопасности
    /// </summary>
    public interface ISecObject
    {
        /// <summary>
        /// Идентификатор объекта в базе данных
        /// </summary>
        int IdSecObject { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        string ObjectName { get; }

        /// <summary>
        /// Идентификатор приложения
        /// </summary>
        int IdApplication { get; set; }

        /// <summary>
        /// Идентификатор типа доступа
        /// </summary>
        int IdAccessType { get; set; }

        /// <summary>
        /// Идентификатор типа доступа
        /// </summary>
//        int? IdAccessType { get; set; }

        /// <summary>
        /// Список разрешений, в котором задействован данный объект
        /// </summary>
        IList<IGrant> Grants { get; }

        /// <summary>
        /// Наименование приложения
        /// </summary>
        IApplication Application { get; set; }

        /// <summary>
        /// Тип доступа
        /// </summary>
        IAccessType AccessType { get; set; }
    }
}