using System;

namespace Security.Interfaces
{
    public interface ICheckAccess
    {
        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns></returns>
        bool CheckAccess(string login, string secObjectName, Enum accessType);
    }
}