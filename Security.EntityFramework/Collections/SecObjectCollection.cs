using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Security.EntityDal;
using Security.EntityFramework.Exceptions;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;
using Security.Model;

namespace Security.EntityFramework.Collections
{
    /// <summary>
    /// Класс, представляющий собой коллекцию объектов безопасности
    /// </summary>
    public class SecObjectCollection : BaseCollection<SecObject>, ISecObjectCollection
    {
        private readonly Application _application;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        /// <param name="application"></param>
        internal SecObjectCollection(SecurityContext context, IApplication application) 
            : base(context)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            _application = (Application) application;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ISecObject> GetEnumerator()
        {
            return SecObjects.GetEnumerator();
        }

        private IQueryable<SecObject> SecObjects
        {
            get
            {
                return Context.SecObjects.AsNoTracking().Where(e => e.IdApplication == _application.IdApplication);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Добавляет новый объект безопасности
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="accessTypeName"></param>
        public void Add(string objectName, string accessTypeName)
        {
            var idAccessType = Context.AccessTypes.Where(at => at.Name == accessTypeName && at.Application.AppName == _application.AppName).Select(at => at.IdAccessType).Single();
            var secObject = new SecObject(){ObjectName = objectName, IdAccessType = idAccessType};
            Add(secObject);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(ISecObject item)
        {
            var secObject = new SecObject()
            {
                Application = _application,
                ObjectName = item.ObjectName,
                IdAccessType = item.IdAccessType
            };

            Context.SecObjects.Add(secObject);
        }

        /// <inheritdoc />
        public void AddRange(IEnumerable<string> objectNames, IEnumerable<string> accessTypeNames)
        {
            var objectNamesEnumerable = objectNames as string[] ?? objectNames.ToArray();
            foreach (var accessTypeName in accessTypeNames)
            {
                foreach (var objectName in objectNamesEnumerable)
                {
                    Add(objectName, accessTypeName);
                }
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            foreach (var secObject in SecObjects)
            {
                Context.SecObjects.Remove(secObject);
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(ISecObject item)
        {
            return SecObjects.Any(e => e.ObjectName == item.ObjectName);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(ISecObject[] array, int arrayIndex)
        {
            Array.Copy(SecObjects.ToArray(), array, 0);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        /// <exception cref="SecurityObjectNotFoundException"></exception>
        public bool Remove(ISecObject item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var secObject = Context.SecObjects.Find(item.IdSecObject);
            if (secObject == null)
                throw new SecurityObjectNotFoundException(item.ObjectName);

            return Context.SecObjects.Remove(secObject) != null;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count => SecObjects.Count();

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly => true;

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
        public Expression Expression => SecObjects.Expression;

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"/> is executed.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.
        /// </returns>
        public Type ElementType => SecObjects.ElementType;

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.IQueryProvider"/> that is associated with this data source.
        /// </returns>
        public IQueryProvider Provider => SecObjects.Provider;

        /// <summary>
        /// Производит обновление элемента в БД
        /// </summary>
        /// <param name="item">Элемент <see cref="T"/></param>
        public void Update(ISecObject item)
        {
            Context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Производит удаление элементов представленных коллекцией <see cref="items"/>
        /// </summary>
        /// <param name="items">Коллекция удаляемых элементов</param>
        /// <returns>Коллекцию удаленных элементов</returns>
        public IEnumerable<ISecObject> RemoveRange(IEnumerable<ISecObject> items)
        {
            var ids = items.Select(e => e.IdSecObject);
            var secObjects = Context.SecObjects.Where(e => ids.Contains(e.IdSecObject));
            return Context.SecObjects.RemoveRange(secObjects);

        }
    }
}