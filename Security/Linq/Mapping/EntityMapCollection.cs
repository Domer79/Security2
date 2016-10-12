using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Security.Linq.Mapping
{
    internal class EntityMapCollection : IEnumerable<EntityMap>
    {
        private readonly List<EntityMap> _entityMaps = new List<EntityMap>();

        public void Add<TInterface>(Type entityType) where TInterface : class
        {
            if (_entityMaps.Any(em => em.CheckExistsEntityOrInterfaceType(entityType, typeof(TInterface))))
                throw new InvalidOperationException("Такой тип уже зарегистрирован");

            var entityMap = new EntityMap() {EntityType = entityType, InterfaceType = typeof(TInterface)};
            _entityMaps.Add(entityMap);
        }

        public bool ContainsEntityType(Type entityType)
        {
            return _entityMaps.Any(t => t.EntityType == entityType);
        }

        public bool ContainsInterfaceType(Type interfaceType)
        {
            return _entityMaps.Any(t => t.InterfaceType == interfaceType);
        }

        public Type this[Type interfaceType]
        {
            get { return GetEntityType(interfaceType); }
        }

        public Type GetInterfaceType(Type entityType)
        {
            return _entityMaps.First(t => t.EntityType == entityType).InterfaceType;
        }

        private Type GetEntityType(Type interfaceType)
        {
            return _entityMaps.First(t => t.InterfaceType == interfaceType).EntityType;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<EntityMap> GetEnumerator()
        {
            return _entityMaps.GetEnumerator();
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
    }
}