using System.Collections.Generic;
using Security.Interfaces.Base;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    public interface IUserCollection : IQueryableCollection<IUser>
    {
        /// <summary>
        /// Идентификация пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool LogIn(string login, string password);
    }
}
