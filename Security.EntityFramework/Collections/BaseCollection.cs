using System.Data.Entity;
using Security.EntityDal;

namespace Security.EntityFramework.Collections
{
    /// <summary>
    /// Базовая коллекция элементов сущностей модели системы доступа
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseCollection<T> where T : class
    {
        protected readonly SecurityContext Context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст <see cref="DbContext"/></param>
        protected BaseCollection(SecurityContext context)
        {
            Context = context;
        }
    }
}
