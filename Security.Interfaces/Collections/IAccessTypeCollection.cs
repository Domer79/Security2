using System.Collections.Generic;
using Security.Interfaces.Base;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    /// <summary>
    /// Интерфейс, представляющий коллекцию типов доступа
    /// </summary>
    public interface IAccessTypeCollection : IQueryableCollection<IAccessType>
    {
    }
}