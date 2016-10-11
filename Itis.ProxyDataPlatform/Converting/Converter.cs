using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Itis.Common.Extensions;
using Itis.ProxyDataPlatform.Attributes;
using Itis.ProxyDataPlatform.Base;
using Itis.ProxyDataPlatform.DatabaseContext;
using Itis.ProxyDataPlatform.Exceptions;
using Itis.ProxyDataPlatform.Infrastructure;
using Itis.ProxyDataPlatform.Interfaces;

namespace Itis.ProxyDataPlatform.Converting
{
    public static class Converter
    {
        /// <summary>
        /// Карта маппинга bll-типа в тип dal. В качестве ключа используется тип bll
        /// </summary>
        private static readonly Dictionary<Type, Type> _entityTypeMaps = new Dictionary<Type, Type>();
        static readonly DefaultConverter DefaultConverter = new DefaultConverter();
        private static readonly object ConvertersLockObject = new object();
        public static readonly object EntityTypeMapsLockObject = new object();
        private static ConverterCollection _converters;

        public static Type BllTypeMap(Type bllEntityType)
        {
            if (EntityTypeMaps.ContainsKey(bllEntityType))
                return EntityTypeMaps[bllEntityType];

            var mapToAttribute = bllEntityType.GetAttribute<MapToAttribute>();

            if (mapToAttribute != null) 
                return mapToAttribute.EntityBllType;

            if (bllEntityType.Is<EntityBll>())
                throw new EntityMapToBllMissingException(string.Format("Отсутствует соответствие типа сушности для типа прокси-класса {0}", bllEntityType));
                
            throw new EntityMapToBllMissingException(string.Format("Отсутствует соответствие типа прокси-класса для типа сушности {0}", bllEntityType));
        }

        internal static Type BllTypeMap(object entity)
        {
            var sourceType = entity.GetType();

            if (sourceType.Namespace == "System.Data.Entity.DynamicProxies")
                sourceType = ObjectContext.GetObjectType(sourceType);

            return BllTypeMap(sourceType);
        }

        public static void AddDataModelAssembly(Assembly assembly)
        {
            ConvertersInit(assembly);
            EntityTypeMapInit(assembly);
        }

        #region Async Members

        public static void AddDataModelAssemblyAsync(Assembly assembly)
        {
            Task.Run(() => ConvertersInit(assembly));
            Task.Run(() => EntityTypeMapInit(assembly));
        }

        #endregion

        private static void ConvertersInit(Assembly assembly)
        {
            lock (ConvertersLockObject)
            {
                if (_converters == null)
                    _converters = new ConverterCollection();

                _converters.AddConverterAssembly(assembly);
            }
        }

        private static void EntityTypeMapInit(Assembly assembly)
        {
            lock (EntityTypeMapsLockObject)
            {
                var entityTypes = assembly.GetTypes().Where(t => t.Is<Entity>());
                foreach (var entityType in entityTypes)
                {
                    var mapToAttribute = entityType.GetAttribute<MapToAttribute>();
                    if (mapToAttribute == null)
                        continue;

                    if (_entityTypeMaps.ContainsKey(mapToAttribute.EntityBllType))
                        continue;

                    _entityTypeMaps.Add(mapToAttribute.EntityBllType, entityType);
                }
            }
        }

        internal static ConverterCollection Converters
        {
            get
            {
                lock (ConvertersLockObject)
                {
                    return _converters ?? (_converters = new ConverterCollection());
                }
            }
        }

        /// <summary>
        /// Карта маппинга bll-типа в тип dal. В качестве ключа используется тип bll
        /// </summary>
        internal static Dictionary<Type, Type> EntityTypeMaps
        {
            get
            {
                lock (EntityTypeMapsLockObject)
                {
                    return _entityTypeMaps;
                }
            }
        }

        public static TEntityBll Convert<TEntityBll>(Entity entity) 
            where TEntityBll : EntityBll
        {
            return (TEntityBll) Convert(entity);
        }

        public static TEntity ReConvert<TEntity>(EntityBll entity, BaseContainer container, bool fromId = false)
            where TEntity: Entity
        {
            return (TEntity)ReConvert(entity, container, fromId);
        }

        public static T Convert<T>(object entity)
            where T : class 
        {
            if (entity.GetType() == typeof (T))
                return (T)entity;

            return (T)Convert(entity);
        }

        public static T ReConvert<T>(object entityBll, BaseContainer container, bool fromId = false)
            where T : class
        {
            if (entityBll.GetType() == typeof (T))
                return (T)entityBll;

            return (T)ReConvert(entityBll, container, fromId);
        }

        public static object Convert(object entity)
        {
            if (entity == null)
                return null;

            var stackPushed = false;
            try
            {
                if (EntityStack.ExistKey(entity))
                    return (EntityBll)EntityStack.GetEntity(entity);

                var converter = Converters[ObjectContext.GetObjectType(entity.GetType())];
                EntityStack.Push(entity, null);
                stackPushed = true;
                var entityBll = (EntityBll)converter.Convert(entity);

                return entityBll;
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    return DefaultConverter.Convert((Entity)entity);
                }
                catch (Exception ex)
                {
                    throw new InvalidConvertOperationException(
                        "Произошла ошибка при преобразовании объекта DAL в объект BLL.", ex);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidConvertOperationException(
                    "Произошла ошибка при преобразовании объекта DAL в объект BLL.", ex);
            }
            finally
            {
                if (stackPushed)
                    EntityStack.Pop();
            }
        }

        public static object ReConvert(object entityBll, BaseContainer container, bool fromId = false)
        {
            if (container == null)
                return null;

            if (entityBll == null)
                return null;

            var entityType = BllTypeMap(entityBll.GetType());
            var stackPushed = false;
            try
            {
                /*********************TODO: Протестировать!!!************************/
                if (EntityStack.ExistKey(entityBll))
                    return EntityStack.GetEntity(entityBll);
                /*********************TODO: Протестировать!!!************************/

                if (fromId)
                    return container.Set(entityType).Find(((EntityBll)entityBll).Id);

                var converter = Converters[entityType];
                EntityStack.Push(entityBll, null);
                stackPushed = true;
                return converter.ReConvert(entityBll, container);
                //                return (Entity)converter.GetType().GetMethod("ReConvert").Invoke(converter, new object[] { entity, container });
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    return DefaultConverter.ReConvert((EntityBll)entityBll, entityType);
                }
                catch (Exception ex)
                {
                    throw new InvalidConvertOperationException(
                        "Произошла ошибка при преобразовании объекта DAL в объект BLL.", ex);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidConvertOperationException(
                    "Произошла ошибка при преобразовании объекта DAL в объект BLL.", ex);
            }
            finally
            {
                if (stackPushed)
                    EntityStack.Pop();
            }
        }

        public static void ReConvertToDal(object entityBll, object entity, BaseContainer container)
        {
            if (entityBll == null)
                throw new ArgumentNullException("entityBll");
            if (entity == null)
                throw new ArgumentNullException("entity");

            var entityType = BllTypeMap(entityBll.GetType());
            try
            {
                var converter = Converters[entityType];
                EntityStack.Push(entityBll, null);
                converter.ReConvertToDal(entityBll, entity, container);
                //                return (Entity)converter.GetType().GetMethod("ReConvert").Invoke(converter, new object[] { entity, container });
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    DefaultConverter.ReConvertToDal((EntityBll)entityBll, (Entity) entity, entityType);
                }
                catch (Exception ex)
                {
                    throw new InvalidConvertOperationException(
                        "Произошла ошибка при преобразовании объекта DAL в объект BLL.", ex);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidConvertOperationException(
                    "Произошла ошибка при преобразовании объекта DAL в объект BLL.", ex);
            }
            finally
            {
                EntityStack.Pop();
            }
        }

        internal static void EntityConvert(IEntity fromEntity, IEntity toEntity)
        {
            toEntity.Id = fromEntity.Id;
            toEntity.DateCreated = fromEntity.DateCreated;
            toEntity.DateUpdated = fromEntity.DateUpdated;
        }

        internal static TOut CreateInstance<TOut, TIn>(TIn entity)
        {
            return (TOut) CreateInstance(entity);
        }

        internal static object CreateInstance(object entity)
        {
            var entityInstance = Activator.CreateInstance(BllTypeMap(entity));
            EntityStack.Set(entity, entityInstance);

            return entityInstance;
        }

//        internal static object CreateInstance(object entity, BaseContainer container)
//        {
//            var entityInstance = Activator.CreateInstance(container.Converter.BllTypeMap(entity));
//            container.Converter.EntityStack.Add(entity, entityInstance);
//
//            return entityInstance;
//        }
//
    }
}
