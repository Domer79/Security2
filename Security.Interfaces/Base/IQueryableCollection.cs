using System;
using System.Collections.Generic;
using System.Linq;

namespace Security.Interfaces.Base
{
    public interface IQueryableCollection<T> : ICollection<T>, ISecurityQueryable<T>, ISavedCollection
    {
        /// <summary>
        /// Производит обновление элемента в БД
        /// </summary>
        /// <param name="item">Элемент <see cref="T"/></param>
        void Update(T item);

        /// <summary>
        /// Производит удаление элементов представленных коллекцией <see cref="items"/>
        /// </summary>
        /// <param name="items">Коллекция удаляемых элементов</param>
        /// <returns>Коллекцию удаленных элементов</returns>
        IEnumerable<T> RemoveRange(IEnumerable<T> items);
    }
}