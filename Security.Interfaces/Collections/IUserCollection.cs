using System.Collections.Generic;
using Security.Interfaces.Base;
using Security.Interfaces.Model;
using System.Linq;

namespace Security.Interfaces.Collections
{
    /// <summary>
    /// Интерфейс, представляющий собой коллекцию пользователй
    /// </summary>
    public interface IUserCollection : IQueryableCollection<IUser>
    {
        /// <summary>
        /// Возвращает коллекцию пользователей, при этом не учитывая полученные данные
        /// </summary>
        /// <returns></returns>
        IQueryable<IUser> AsNoTracking();
    }
}
