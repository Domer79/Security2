using System;

namespace Security.Linq.Mapping
{
    internal class EntityMap
    {
        public Type EntityType { get; set; }
        public Type InterfaceType { get; set; }

        public bool CheckExistsEntityOrInterfaceType(Type entityType, Type interfaceType)
        {
            return EntityType == entityType || InterfaceType == entityType || EntityType == interfaceType || InterfaceType == interfaceType;
        }
    }
}