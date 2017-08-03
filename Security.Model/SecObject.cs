using System;
using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Model
{
    /// <summary>
    /// Объект безопасности
    /// </summary>
    public class SecObject : ISecObject
    {
        /// <summary>
        /// Идентификатор объекта в базе данных
        /// </summary>
        public int IdSecObject { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Идентификатор приложения
        /// </summary>
        public int IdApplication { get; set; }

        /// <summary>
        /// Идентификатор типа доступа
        /// </summary>
        public int IdAccessType { get; set; }

        /// <summary>
        /// Список разрешений, в котором задействован данный объект
        /// </summary>
        public HashSet<Grant> Grants { get; set; }

        /// <summary>
        /// Приложения
        /// </summary>
        public Application Application { get; set; }

        /// <summary>
        /// Тип доступа
        /// </summary>
        public AccessType AccessType { get; set; }

        /// <summary>
        /// Наименование приложения
        /// </summary>
        IApplication ISecObject.Application
        {
            get { return Application; }
            set { Application = (Application) value; }
        }

        /// <summary>
        /// Тип доступа
        /// </summary>
        IAccessType ISecObject.AccessType
        {
            get { return AccessType; }
            set { AccessType = (AccessType)value; }
        }

        /// <summary>
        /// Список разрешений, в котором задействован данный объект
        /// </summary>
        IList<IGrant> ISecObject.Grants => new List<IGrant>(Grants);

        /// <summary>
        /// Возвращает строку, которая представляет текущий объект.
        /// </summary>
        /// <returns>
        /// Строка, представляющая текущий объект.
        /// </returns>
        public sealed override string ToString()
        {
            return ObjectName;
        }
    }
}
