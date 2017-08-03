using System.Collections.Generic;
using Security.Interfaces.Base;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    /// <summary>
    /// Интерфейс, представляющий собой коллекцию объектов безопасности
    /// </summary>
    public interface ISecObjectCollection : IQueryableCollection<ISecObject>
    {
        /// <summary>
        /// Добавляет список объектов безопасности в БД
        /// </summary>
        /// <param name="objectNames"></param>
        /// <param name="accessTypeNames"></param>
        void AddRange(IEnumerable<string> objectNames, IEnumerable<string> accessTypeNames);

        /// <summary>
        /// Добавляет новый объект безопасности
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="accessTypeName"></param>
        void Add(string objectName, string accessTypeName);
    }
}