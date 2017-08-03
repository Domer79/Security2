using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Model
{
    /// <summary>
    /// Тип доступа
    /// </summary>
    public class AccessType : IAccessType
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public AccessType()
        {
            Grants = new HashSet<Grant>();
        }

        /// <summary>
        /// Идентификатор типа доступа в БД
        /// </summary>
        public int IdAccessType { get; set; }

        /// <summary>
        /// Наименование типа доступа, сопоставляется с наименованием перечислителя в клиентском ПО
        /// </summary>
        public string Name { get; set; }

        public int IdApplication { get; set; }

        /// <summary>
        /// Список разрешений
        /// </summary>
        public HashSet<Grant> Grants { get; set; }

        /// <summary>
        /// Приложение
        /// </summary>
        public Application Application { get; set; }

        /// <summary>
        /// Привязанные к типу доступа объекты безопасности
        /// </summary>
        public HashSet<SecObject> SecObjects { get; set; }

        /// <summary>
        /// Список разрешений
        /// </summary>
        IList<IGrant> IAccessType.Grants => new List<IGrant>(Grants);

        /// <summary>
        /// Привязанные к типу доступа объекты безопасности
        /// </summary>
        IList<ISecObject> IAccessType.SecObjects => new List<ISecObject>();

        /// <summary>
        /// Приложение
        /// </summary>
        IApplication IAccessType.Application
        {
            get { return Application; }
            set { Application = (Application) value; }
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            return IdAccessType.GetHashCode();
        }
    }
}
