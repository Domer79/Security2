using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Security.EntityDal;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;
using Security.Model;

namespace Security.EntityFramework.Collections
{
    /// <summary>
    /// Класс, представляющий коллекцию типов доступа
    /// </summary>
    public class AccessTypeCollection : BaseCollection<AccessType>, IAccessTypeCollection
    {
        private readonly Application _application;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст <see cref="DbContext"/></param>
        /// <param name="application"></param>
        internal AccessTypeCollection(SecurityContext context, IApplication application) 
            : base(context)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            _application = (Application) application;
        }

        private IQueryable<AccessType> AccessTypes
        {
            get
            {
                return Context.AccessTypes.AsNoTracking().Include("Application").Where(e => e.Application.AppName.Equals(_application.AppName, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Возвращает перечислитель, выполняющий перебор элементов коллекции.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IAccessType> GetEnumerator()
        {
            return AccessTypes.GetEnumerator();
        }

        /// <summary>
        /// Возвращает перечислитель, выполняющий перебор элементов коллекции.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Добавляет элемент в коллекцию <see cref="AccessTypeCollection"/>.
        /// </summary>
        /// <param name="item">Объект <see cref="IAccessType"/></param>
        public void Add(IAccessType item)
        {

            Context.AccessTypes.Add((AccessType)item);
        }

        /// <summary>
        /// Удаляет все из коллекции <see cref="AccessTypeCollection"/>.
        /// </summary>
        public void Clear()
        {
            foreach (var accessType in AccessTypes)
            {
                Context.AccessTypes.Remove(accessType);
            }
        }

        /// <summary>
        /// Определяет в коллекции <see cref="AccessTypeCollection"/> содержание элемента <see cref="IAccessType"/>.
        /// </summary>
        /// <returns>
        /// true если <paramref name="item"/> найден в <see cref="AccessTypeCollection"/>, иначе false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(IAccessType item)
        {
            return AccessTypes.Any(a => a.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(IAccessType[] array, int arrayIndex)
        {
            Array.Copy(AccessTypes.ToArray(), array, 0);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(IAccessType item)
        {
            var accessType = Context.AccessTypes.Remove((AccessType) item);
            return accessType != null;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count => AccessTypes.Count();

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly => false;

        /// <summary>
        /// Производит сохранение всех изменений в базу данных
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.Expressions.Expression"/> that is associated with this instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </returns>
        public Expression Expression => AccessTypes.Expression;

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"/> is executed.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.
        /// </returns>
        public Type ElementType => AccessTypes.ElementType;

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.IQueryProvider"/> that is associated with this data source.
        /// </returns>
        public IQueryProvider Provider => AccessTypes.Provider;

        /// <summary>
        /// Производит обновление элемента в БД
        /// </summary>
        /// <param name="item">Элемент <see cref="T"/></param>
        public void Update(IAccessType item)
        {
            Context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Производит удаление элементов представленных коллекцией <see cref="items"/>
        /// </summary>
        /// <param name="items">Коллекция удаляемых элементов</param>
        /// <returns>Коллекцию удаленных элементов</returns>
        public IEnumerable<IAccessType> RemoveRange(IEnumerable<IAccessType> items)
        {
            var ids = items.Select(e => e.IdAccessType);
            var accessTypes = AccessTypes.Where(e => ids.Contains(e.IdAccessType));
            return Context.AccessTypes.RemoveRange(accessTypes);
        }
    }
}