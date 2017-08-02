using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Security.EntityDal.Base;

namespace Security.EntityDal.Infrastructure
{
    /// <summary>
    /// Класс информатор для сущности
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityInfo<TEntity> where TEntity : class
    {
        private readonly RepositoryDataContext _context;
        private string _keyName;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <exception cref="ArgumentNullException">Если контекст равен null</exception>
        public EntityInfo(RepositoryDataContext context)
        {
            if (context == null) 
                throw new ArgumentNullException("context");

            _context = context;
        }

        private ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter) _context).ObjectContext; }
        }

        /// <summary>
        /// Возвращает имя свойства, помеченного атрибутом <see cref="KeyAttribute"/>
        /// </summary>
        public string KeyName
        {
            get { return _keyName ?? (_keyName = GetEntityKeyName()); }
        }

        string GetEntityKeyName()
        {
            var set = ObjectContext.CreateObjectSet<TEntity>();
            var entitySet = set.EntitySet;
            return entitySet.ElementType.KeyMembers[0].Name;
        }

        /// <summary>
        /// Возвращает выражение для ключа сущности
        /// </summary>
        /// <typeparam name="TKey">Тип ключа</typeparam>
        /// <returns>Выражение вида <code>p => p.Key</code></returns>
        public Expression<Func<TEntity, TKey>> GetMemberAccess<TKey>()
        {
            return GetMemberAccess<TKey>(null);
        }

        /// <summary>
        /// Возвращает выражение для свойства сущности
        /// </summary>
        /// <typeparam name="TKey">Тип свойства</typeparam>
        /// <param name="columnName">Имя свойства</param>
        /// <returns>Выражение вида <code>p => p.Key</code></returns>
        public Expression<Func<TEntity, TKey>> GetMemberAccess<TKey>(string columnName)
        {
            var parameter = Expression.Parameter(typeof (TEntity), "p");
            var memberAccess = Expression.Property(parameter, string.IsNullOrEmpty(columnName) ? KeyName : columnName);
            return (Expression<Func<TEntity, TKey>>)Expression.Lambda(memberAccess, parameter);
        }

        /// <summary>
        /// Возвращает выражение для свойства сущности, тип которого неизвестен
        /// </summary>
        /// <param name="columnName">Имя свойства</param>
        /// <returns>Выражение вида <code>p => p.Key</code></returns>
        public Expression<Func<TEntity, object>> GetMemberAccess(string columnName)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "p");
            var memberAccess = Expression.Property(parameter, string.IsNullOrEmpty(columnName) ? KeyName : columnName);
            return (Expression<Func<TEntity, object>>)Expression.Lambda(memberAccess, parameter);
        }

        /// <summary>
        /// Возвращает информацию о ключе
        /// </summary>
        /// <returns>Возвращает объект типа <see cref="PropertyInfo"/></returns>
        public PropertyInfo GetKeyInfo()
        {
            var keyInfo = typeof (TEntity).GetProperties().FirstOrDefault(pi => pi.Name == KeyName);

            if (keyInfo == null) 
                throw new KeyNotFoundException("KeyName");

            return keyInfo;
        }

        /// <summary>
        /// Имя сущности
        /// </summary>
        public string EntityName 
        {
            get { return typeof(TEntity).Name; }
        }

        #region GetTableName

        /// <summary>
        /// Имя сопоставленной таблицы в базе данных
        /// </summary>
        public string TableName
        {
            get { return Tools.GetTableName<TEntity>(_context); }
        }

        #endregion
    }
}