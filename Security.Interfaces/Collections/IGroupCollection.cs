using System.Collections.Generic;
using Security.Interfaces.Base;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    /// <summary>
    /// Интерфейс, представляющий собой коллекцию групп пользователей
    /// </summary>
    public interface IGroupCollection : IQueryableCollection<IGroup>
    {
    }
}