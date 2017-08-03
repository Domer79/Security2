using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Security.EntityDal;
using Security.EntityFramework.Exceptions;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;
using Security.Model;

namespace Security.EntityFramework.Collections
{
    /// <summary>
    /// Класс, представляющий собой коллекцию разрешений
    /// </summary>
    public class GrantCollection : BaseCollection<Grant>, IGrantCollection
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="securityContext">Контекст <see cref="DbContext"/></param>
        internal GrantCollection(SecurityContext securityContext) 
            : base(securityContext)
        {
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IGrant> GetEnumerator()
        {
            return ((IQueryable<Grant>)Context.Grants.AsNoTracking()).GetEnumerator();
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
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(IGrant item)
        {
            Context.Grants.Add((Grant) item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            foreach (var grant in Context.Grants)
            {
                Context.Grants.Remove(grant);
            }
        }

        /// <summary>
        /// Возвращает объект разрешения по наименованию объекта безопасности, роли, типа доступа и приложения
        /// </summary>
        /// <param name="secObject">Объект безопасности</param>
        /// <param name="role">Роль</param>
        /// <param name="accessType">Тип доступа</param>
        /// <param name="appName">Имя приложения</param>
        /// <returns></returns>
        /// <exception cref="GrantNotFoundException"></exception>
        public IGrant Get(string secObject, string role, string accessType, string appName)
        {
            var app = Context.Applications.Single(_ => _.AppName == appName);
            var so = Context.SecObjects.Single(_ => _.ObjectName == secObject && _.IdApplication == app.IdApplication);
            var r = Context.Roles.Single(_ => _.Name == role && _.IdApplication == app.IdApplication);
            var at = Context.AccessTypes.Single(_ => _.Name == accessType && _.IdApplication == app.IdApplication);

            var grant = Context.Grants.Find(so.IdSecObject, r.IdRole, at.IdAccessType);

            if (grant == null)
                throw new GrantNotFoundException(so.IdSecObject, r.IdRole, at.IdAccessType);

            return grant;
        }

        /// <summary>
        /// Асинхронно Возвращает объект разрешения по наименованию объекта безопасности, роли, типа доступа и приложения
        /// </summary>
        /// <param name="secObject"></param>
        /// <param name="role"></param>
        /// <param name="accessType"></param>
        /// <param name="appName"></param>
        /// <returns></returns>
        /// <exception cref="GrantNotFoundException"></exception>
        public async Task<IGrant> GetAsync(string secObject, string role, string accessType, string appName)
        {
            var app = Context.Applications.Single(_ => _.AppName == appName);
            var so = Context.SecObjects.SingleAsync(_ => _.ObjectName == secObject && _.IdApplication == app.IdApplication);
            var r = Context.Roles.SingleAsync(_ => _.Name == role && _.IdApplication == app.IdApplication);
            var at = Context.AccessTypes.SingleAsync(_ => _.Name == accessType && _.IdApplication == app.IdApplication);

            var idSecObject = (await so).IdSecObject;
            var idRole = (await r).IdRole;
            var idAccessType = (await at).IdAccessType;
            var grant = Context.Grants.Find(idSecObject, idRole, idAccessType);

            if (grant == null)
                throw new GrantNotFoundException(idSecObject, idRole, idAccessType);

            return grant;
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(IGrant item)
        {
            return Context.Grants.Find(item.IdSecObject, item.IdRole, item.IdAccessType) != null;
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(IGrant[] array, int arrayIndex)
        {
            Array.Copy(Context.Grants.ToArray(), array, 0);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(IGrant item)
        {
            var grant = Context.Grants.Remove((Grant) item);
            return grant != null;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count => Context.Grants.Count();

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
        /// Добавляет новое разрешение в базу данных
        /// </summary>
        /// <param name="roleName">Имя роль</param>
        /// <param name="secObjectName">Наименование объекта безопасности</param>
        /// <param name="accessTypeName">Наименование типа доступа</param>
        /// <param name="applicationName"></param>
        public IGrant Add(string roleName, string secObjectName, string accessTypeName, string applicationName)
        {
            var idApplication = Context.Applications.AsNoTracking().First(e => e.AppName.Equals(applicationName, StringComparison.OrdinalIgnoreCase)).IdApplication;

            var grant = new Grant();
            grant.SecObject = Context.SecObjects.First(so => so.ObjectName.Equals(secObjectName, StringComparison.OrdinalIgnoreCase) && so.IdApplication == idApplication);
            grant.Role = Context.Roles.First(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase) && r.IdApplication == idApplication);
            grant.AccessType = Context.AccessTypes.First(a => a.Name.Equals(accessTypeName, StringComparison.OrdinalIgnoreCase) && a.IdApplication == idApplication);

            return Context.Grants.Add(grant);
        }

        public IEnumerable<IGrant> AddRange(string roleName, ISecObject[] secObjects, IAccessType[] accessTypes, string applicationName)
        {
            var idApplication = Context.Applications.AsNoTracking().First(e => e.AppName.Equals(applicationName, StringComparison.OrdinalIgnoreCase)).IdApplication;

            var idSecObjects = secObjects.Select(e => e.IdSecObject);

            var idAccessTypes = accessTypes.Select(e => e.IdAccessType);

            var idRole = Context.Roles
                .AsNoTracking()
                .Where(e => e.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase) && e.IdApplication == idApplication)
                .Select(e => e.IdRole)
                .First();

            var grants = new List<Grant>();
            foreach (var idAccessType in idAccessTypes)
            {
                foreach (var idSecObject in idSecObjects)
                {
                    grants.Add(new Grant() { IdRole = idRole, IdSecObject = idSecObject, IdAccessType = idAccessType });
                }
            }

            return Context.Grants.AddRange(grants);
        }

        /// <summary>
        /// Удаляет разрешение из базы данных
        /// </summary>
        /// <param name="roleName">Имя роль</param>
        /// <param name="secObjectName">Наименование объекта безопасности</param>
        /// <param name="accessTypeName">Наименование типа доступа</param>
        /// <param name="applicationName"></param>
        /// <returns>Количество удаленных элементов</returns>
        public async Task<bool> RemoveAsync(string roleName, string secObjectName, string accessTypeName, string applicationName)
        {
            var idApplication = Context.Applications.Where(e => e.AppName == applicationName).Select(e => e.IdApplication).First();

            var secObject = Context.SecObjects
                    .AsNoTracking()
                    .FirstAsync(e => e.ObjectName == secObjectName && e.IdApplication == idApplication);

            var accessType = Context.AccessTypes
                    .AsNoTracking()
                    .FirstAsync(e => e.Name == accessTypeName && e.IdApplication == idApplication);

            var role = Context.Roles.AsNoTracking().FirstAsync(e => e.Name == roleName && e.IdApplication == idApplication);

            var idSecObject = (await secObject).IdSecObject;
            var idAccessType = (await accessType).IdAccessType;
            var idRole = (await role).IdRole;

            var grant = Context.Grants.AsNoTracking()
                .FirstAsync(g => g.IdRole == idRole && g.IdAccessType == idAccessType && g.IdSecObject == idSecObject);

            return await Task.Run(async () => Context.Grants.Remove(await grant) != null);
        }

        /// <summary>
        /// Удаляет разрешение из базы данных
        /// </summary>
        /// <param name="roleName">Имя роль</param>
        /// <param name="secObjectName">Наименование объекта безопасности</param>
        /// <param name="accessTypeName">Наименование типа доступа</param>
        /// <param name="applicationName"></param>
        /// <returns>Количество удаленных элементов</returns>
        public bool Remove(string roleName, string secObjectName, string accessTypeName, string applicationName)
        {
            var idApplication = Context.Applications.Where(e => e.AppName == applicationName).Select(e => e.IdApplication).First();

            var idSecObject =
                Context.SecObjects
                    .AsNoTracking()
                    .Where(e => e.ObjectName == secObjectName && e.IdApplication == idApplication)
                    .Select(e => e.IdSecObject)
                    .First();

            var idAccessType = Context.AccessTypes
                    .AsNoTracking()
                    .Where(e => e.Name == accessTypeName && e.IdApplication == idApplication)
                    .Select(e => e.IdAccessType)
                    .First();

            var idRole = Context.Roles
                .AsNoTracking()
                .Where(e => e.Name == roleName && e.IdApplication == idApplication)
                .Select(e => e.IdRole)
                .First();

            var grant = Context.Grants
                .First(g => g.IdRole == idRole && g.IdAccessType == idAccessType && g.IdSecObject == idSecObject);

            return Context.Grants.Remove(grant) != null;
        }

        public IEnumerable<IGrant> RemoveRange(string[] roleNames, string[] secObjects, string[] accessTypes, string applicationName)
        {
            var idApplication = Context.Applications.Where(e => e.AppName == applicationName).Select(e => e.IdApplication).First();

            var idSecObjects =
                Context.SecObjects
                    .AsNoTracking()
                    .Where(e => secObjects.Contains(e.ObjectName) && e.IdApplication == idApplication)
                    .Select(e => e.IdSecObject);

            var idAccessTypes = Context.AccessTypes
                    .AsNoTracking()
                    .Where(e => accessTypes.Contains(e.Name) && e.IdApplication == idApplication)
                    .Select(e => e.IdAccessType);

            var idRoles = Context.Roles
                .AsNoTracking()
                .Where(e => roleNames.Contains(e.Name) && e.IdApplication == idApplication)
                .Select(e => e.IdRole);

            var grants = Context.Grants
                .Where(g => idRoles.Contains(g.IdRole) && idAccessTypes.Contains(g.IdAccessType) && idSecObjects.Contains(g.IdSecObject));

            return Context.Grants.RemoveRange(grants);
        }

        /// <summary>
        /// Добавляет новое разрешение в базу данных
        /// </summary>
        /// <param name="roleName">Имя роль</param>
        /// <param name="secObjectName">Наименование объекта безопасности</param>
        /// <param name="accessType">Тип доступа, представленный в виде <see cref="Enum"/></param>
        /// <param name="applicationName"></param>
        public IGrant Add(string roleName, string secObjectName, Enum accessType, string applicationName)
        {
            return Add(roleName, secObjectName, accessType.ToString(), applicationName);
        }

        /// <summary>
        /// Удаляет разрешение из базы данных
        /// </summary>
        /// <param name="roleName">Имя роль</param>
        /// <param name="secObjectName">Наименование объекта безопасности</param>
        /// <param name="accessType">Тип доступа, представленный в виде <see cref="Enum"/></param>
        /// <param name="applicationName"></param>
        /// <returns>Количество удаленных элементов</returns>
        public bool Remove(string roleName, string secObjectName, Enum accessType, string applicationName)
        {
            return Remove(roleName, secObjectName, accessType.ToString(), applicationName);
        }

        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.Expressions.Expression"/> that is associated with this instance of <see cref="T:System.Linq.IQueryable"/>.
        /// </returns>
        public Expression Expression => ((IQueryable<Grant>) Context.Grants).Expression;

        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"/> is executed.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type"/> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.
        /// </returns>
        public Type ElementType => ((IQueryable<Grant>) Context.Grants).ElementType;

        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.IQueryProvider"/> that is associated with this data source.
        /// </returns>
        public IQueryProvider Provider => ((IQueryable<Grant>) Context.Grants).Provider;

        /// <summary>
        /// Производит обновление элемента в БД
        /// </summary>
        /// <param name="item">Элемент <see cref="T"/></param>
        public void Update(IGrant item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Производит удаление элементов представленных коллекцией <see cref="items"/>
        /// </summary>
        /// <param name="items">Коллекция удаляемых элементов</param>
        /// <returns>Коллекцию удаленных элементов</returns>
        public IEnumerable<IGrant> RemoveRange(IEnumerable<IGrant> items)
        {
            var enumerable = items as IGrant[] ?? items.ToArray();

            var idSecObjects = enumerable.Select(e => e.IdSecObject);
            var idRoles = enumerable.Select(e => e.IdRole);
            var idAccessTypes = enumerable.Select(e => e.IdAccessType);

            var grants = Context.Grants.Where(e => idSecObjects.Contains(e.IdSecObject) && idRoles.Contains(e.IdRole) && idAccessTypes.Contains(e.IdAccessType));
            return Context.Grants.RemoveRange(grants);
        }
    }
}
