using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;

namespace Security.Interfaces
{
    /// <summary>
    /// Интерфейс ядра системы безопасности
    /// </summary>
    public interface ISecurity
    {
        /// <summary>
        /// Коллекция пользователей
        /// </summary>
        IUserCollection UserCollection { get; }

        /// <summary>
        /// Коллекция групп пользователей
        /// </summary>
        IGroupCollection GroupCollection { get; }

        /// <summary>
        /// Коллекция объектов безопасности для текущего приложения
        /// </summary>
        ISecObjectCollection SecObjectCollection { get; }

        /// <summary>
        /// Коллекция ролей для текущего приложения
        /// </summary>
        IRoleCollection RoleCollection { get; }

        /// <summary>
        /// Коллекция участников безопасности
        /// </summary>
        IMemberCollection MemberCollection { get; }

        /// <summary>
        /// Коллекция разрешений для текущего приложения
        /// </summary>
        IGrantCollection GrantCollection { get; }

        /// <summary>
        /// Коллекция приложений, которые обслуживает система
        /// </summary>
        IApplicationCollection ApplicationCollection { get; }

        /// <summary>
        /// Проверка входа по логину и паролю
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool LogIn(string login, string password);

        /// <summary>
        /// Проверка доступа пользователя к объекту безопасности по определенному типу доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns>Возвращает true, если доступ для пользователя разрешен</returns>
        bool CheckAccess(string login, string secObjectName, Enum accessType);

        /// <summary>
        /// Проверка доступа пользователя к объекту безопасности по определенному типу доступа, с указанием приложения, для которого применяется проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <param name="appName">Наименование приложения</param>
        /// <returns></returns>
        bool CheckAccess(string login, string secObjectName, Enum accessType, string appName);

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessTypeName">Тип доступа</param>
        /// <returns>Возвращает true, если доступ для пользователя разрешен</returns>
        bool CheckAccess(string login, string secObjectName, string accessTypeName);

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessTypeName">Тип доступа</param>
        /// <param name="appName"></param>
        /// <returns></returns>
        bool CheckAccess(string login, string secObjectName, string accessTypeName, string appName);

        /// <summary>
        /// Возвращает доступные для пользователя объекты безопасности
        /// </summary>
        /// <param name="accessType"></param>
        /// <returns></returns>
        IEnumerable<string> GetAllowAllSecurityObjects(string accessType);

        /// <summary>
        /// Возвращает доступные для пользователя объекты безопасности
        /// </summary>
        /// <param name="login"></param>
        /// <param name="accessType"></param>
        /// <returns></returns>
        IEnumerable<string> GetAllowSecurityObjects(string login, string accessType);

        /// <summary>
        /// Дополнительные инструменты работы с данными
        /// </summary>
        ISecurityTools Tools { get; }

        /// <summary>
        /// Возвращает новую транзакцию для работы с БД
        /// </summary>
        /// <returns></returns>
        ISecurityTransaction BeginTransaction();

        /// <summary>
        /// Возвращает коллекцию типов доступа
        /// </summary>
        /// <returns></returns>
        IEnumerable<IAccessType> GetAccessTypes();

        /// <summary>
        /// Коллекция и инструментарий работы с настройками
        /// </summary>
        ISecuritySettings Settings { get; }

        /// <summary>
        /// Производит сохранение всех изменений в настройках системы безопасности
        /// </summary>
        /// <returns>Возвращает количество затронутых строк</returns>
        int SaveChanges();

        /// <summary>
        /// Производит асинхронное сохранение всех изменений в настройках системы безопасности
        /// </summary>
        /// <returns>Возвращает количество затронутых строк</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Текущее приложение
        /// </summary>
        IApplication CurrentApplication { get; }
    }
}
