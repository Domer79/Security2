using System;

namespace Itis.ProxyDataPlatform.Attributes
{
    public class MapToAttribute : Attribute
    {
        private readonly Type _entityBllType;

        public MapToAttribute(Type entityBllType)
        {
            _entityBllType = entityBllType;
        }

        public Type EntityBllType
        {
            get { return _entityBllType; }
        }
    }
}