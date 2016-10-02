using System;

namespace Security.Model.Infrastructure
{
    internal class EntityMetadata
    {
        private readonly Type _entityType;

        internal EntityMetadata(Type entityType)
        {
            _entityType = entityType;
        }

        public string EntityName
        {
            get { return _entityType.Name; }
        }

        private TAttribute GetCustomAttribute<TAttribute>() where TAttribute : Attribute
        {
            return (TAttribute)Attribute.GetCustomAttribute(_entityType, typeof(TAttribute));
        }

        internal string TableName { get; set; }

        /// <summary>
        /// Возвращает строку, которая представляет текущий объект.
        /// </summary>
        /// <returns>
        /// Строка, представляющая текущий объект.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Entity: {0}, Table: {1}", _entityType.Name, TableName);
        }
    }
}
