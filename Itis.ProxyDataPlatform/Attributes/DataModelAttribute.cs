using System;

namespace Itis.ProxyDataPlatform.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class DataModelAttribute : Attribute
    {
        private readonly Type _containerType;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Attribute"/>.
        /// </summary>
        public DataModelAttribute(Type containerType)
        {
            _containerType = containerType;
        }

        public Type ContainerType
        {
            get { return _containerType; }
        }
    }
}
