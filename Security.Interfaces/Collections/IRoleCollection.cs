using System.Collections.Generic;
using Security.Interfaces.Base;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    /// <summary>
    /// Интерфейс, представляющий собой коллекцию ролей
    /// </summary>
    public interface IRoleCollection : IQueryableCollection<IRole>
    {
        
    }
}