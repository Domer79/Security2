using System;
using System.Linq;
using Itis.Common.Extensions;
using Itis.ProxyDataPlatform.Attributes;
using Itis.ProxyDataPlatform.Base;
using Itis.ProxyDataPlatform.Exceptions;

namespace Itis.ProxyDataPlatform.Converting
{
    internal class DefaultConverter
    {
        public EntityBll Convert(Entity entity)
        {
            var entityType = entity.GetType();
            var mapToAttribute = entityType.GetAttribute<MapToAttribute>();

            if (mapToAttribute == null)
                throw new InvalidConvertOperationException("Отсутствует атрибут MspToAttribute");

            return (EntityBll)Convert(entity, mapToAttribute.EntityBllType);
        }

        public Entity ReConvert(EntityBll entityBll, Type entityType)
        {
            var entity = (Entity)Convert(entityBll, entityType, "id", "datecreated", "dateupdated");

            //Назначаем идентификатор
            entityBll.Id = entity.Id;
            /////////////////////////

            return entity;
        }

        public Entity ReConvertToDal(EntityBll entityBll, Entity entity, Type entityType)
        {
            var dal = (Entity)Convert(entityBll, entity, "id", "datecreated", "dateupdated");

            //Назначаем идентификатор
            entityBll.Id = dal.Id;
            /////////////////////////

            return dal;
        }

        private static object Convert(object fromObject, Type toType, params string[] excludeFields)
        {
            var toObject = Activator.CreateInstance(toType);
            return Convert(fromObject, toObject, excludeFields);
        }

        private static object Convert(object fromObject, object toObject, params string[] excludeFields)
        {
            var fromObjectProperties = fromObject.GetType().GetProperties();
            var toTypeProperties = toObject.GetType().GetProperties().Where(e => e.CanRead && e.CanWrite);

            foreach (var propertyInfo in toTypeProperties.Where(e => !excludeFields.Contains(e.Name, StringComparer.InvariantCultureIgnoreCase)))
            {
                try
                {
                    if (propertyInfo.PropertyType.IsPrimitive())
                        propertyInfo.SetValue(toObject, fromObjectProperties.First(pi => pi.Name == propertyInfo.Name).GetValue(fromObject));
                }
                catch (Exception ex)
                {
                    throw new InvalidConvertOperationException(string.Format("Произошла ошибка при попытке присвоить значение свойству {0}. Дополнительное сообщение смотрите по внутреннем сообщении.", propertyInfo.Name), ex);
                }
            }

            return toObject;
        }
    }
}