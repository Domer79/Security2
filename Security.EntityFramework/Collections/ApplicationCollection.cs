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
    /// Определяет коллекцию приложений, зарегистрированных в системе доступа
    /// </summary>
    public class ApplicationCollection : BaseCollection<Application>, IApplicationCollection
    {
        public ApplicationCollection(SecurityContext context) 
            : base(context)
        {
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IApplication> GetEnumerator()
        {
            return ((IQueryable<Application>) Context.Applications.AsNoTracking()).GetEnumerator();
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
        /// Добавляет в коллекцию новое приложение
        /// </summary>
        /// <param name="item">Экземпляр объекта приложения</param>
        public void Add(IApplication item)
        {
            Context.Applications.Add((Application) item);
        }

        /// <summary>
        /// Произваодит очистку всех приложений
        /// </summary>
        public void Clear()
        {
            foreach (var application in Context.Applications)
            {
                Context.Applications.Remove(application);
            }
        }

        /// <summary>
        /// Проверяет содержится ли исходный элемент приложения в коллекции
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(IApplication item)
        {
            return Context.Applications.Any(e => e.AppName.Equals(item.AppName));
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(IApplication[] array, int arrayIndex)
        {
            Array.Copy(Context.Applications.ToArray(), array, 0);
        }

        /// <summary>
        /// Производит удаление элемента из коллекции
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(IApplication item)
        {
            var app = Context.Applications.First(e => e.AppName == item.AppName);
            return Context.Applications.Remove(app) != null;
        }

        /// <summary>
        /// Возвращает количество зарегистрированных приложений
        /// </summary>
        public int Count => Context.Applications.Count();

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly => true;

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.Expressions.Expression"/> that is associated with this instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </returns>
        public Expression Expression => ((IQueryable<Application>) Context.Applications).Expression;

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"/> is executed.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.
        /// </returns>
        public Type ElementType => ((IQueryable<Application>) Context.Applications).ElementType;

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.IQueryProvider"/> that is associated with this data source.
        /// </returns>
        public IQueryProvider Provider => ((IQueryable<Application>) Context.Applications).Provider;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        /// <summary>
        /// Производит обновление элемента в БД
        /// </summary>
        /// <param name="item">Элемент <see cref="T"/></param>
        public void Update(IApplication item)
        {
            Context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Производит удаление элементов представленных коллекцией <see cref="items"/>
        /// </summary>
        /// <param name="items">Коллекция удаляемых элементов</param>
        /// <returns>Коллекцию удаленных элементов</returns>
        public IEnumerable<IApplication> RemoveRange(IEnumerable<IApplication> items)
        {
            var appNames = items.Select(e => e.AppName);
            var apps = Context.Applications.Where(e => appNames.Contains(e.AppName));
            return Context.Applications.RemoveRange(apps);
        }
    }
}
