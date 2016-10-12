using System;

namespace Security.Linq.Mapping
{
    public class Mapper
    {
        private readonly EntityMapCollection _entityMapCollection = new EntityMapCollection();
        private static readonly Mapper _instance = new Mapper();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        private Mapper()
        {
        }

        public static Mapper Instance
        {
            get { return _instance; }
        }

        public void Map<TInterface>(Type entityType) where TInterface : class
        {
            _entityMapCollection.Add<TInterface>(entityType);
        }

        public Type GetEntityType(Type interfaceType)
        {
            return _entityMapCollection[interfaceType];
        }

        public Type GetInterfaceType(Type entityType)
        {
            return _entityMapCollection.GetInterfaceType(entityType);
        }

        public bool ContainsEntityType(Type entityType)
        {
            return _entityMapCollection.ContainsEntityType(entityType);
        }

        public bool ContainsInterfaceType(Type interfaceType)
        {
            return _entityMapCollection.ContainsInterfaceType(interfaceType);
        }
    }
}