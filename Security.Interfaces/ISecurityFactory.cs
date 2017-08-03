using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;

namespace Security.Interfaces
{
    /// <summary>
    /// Интерфейс фабрики для предоставления всех необходимых компонентов для работы ядра <see cref="ISecurity"/>
    /// </summary>
    public interface ISecurityFactory : IDisposable
    {
        /// <summary>
        /// Наименование приложения
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// Создает коллекцию типов доступа
        /// </summary>
        /// <returns>Коллекцию типов доступа</returns>
        IAccessTypeCollection CreateAccessTypeCollection();

        /// <summary>
        /// Создает коллекцию разрешений
        /// </summary>
        /// <returns>Коллекцию разрешений</returns>
        IGrantCollection CreateGrantCollection();

        /// <summary>
        /// Создает коллекцию групп пользователей
        /// </summary>
        /// <returns>Коллекцию групп пользователей</returns>
        IGroupCollection CreateGroupCollection();

        /// <summary>
        /// Создает коллекцию участников безопасности
        /// </summary>
        /// <returns>Коллекцию участников безопасности</returns>
        IMemberCollection CreateMemberCollection();

        /// <summary>
        /// Создает коллекцию ролей
        /// </summary>
        /// <returns>Коллекцию ролей</returns>
        IRoleCollection CreateRoleCollection();

        /// <summary>
        /// Создает коллекцию объектов безопасности
        /// </summary>
        /// <returns>Коллекцию объектов безопасности</returns>
        ISecObjectCollection CreateSecObjectCollection();

        /// <summary>
        /// Создает коллекцию пользователей
        /// </summary>
        /// <returns>Коллекцию пользователей</returns>
        IUserCollection CreateUserCollection();

        /// <summary>
        /// Создает коллекцию приложений
        /// </summary>
        /// <returns>Коллекцию приложений</returns>
        IApplicationCollection CreateApplicationCollection();

        /// <summary>
        /// Создает эзкемпляр класса <see cref="ISecurityTools"/>
        /// </summary>
        /// <returns>Объект <see cref="ISecurityTools"/></returns>
        ISecurityTools CreateSecurityTools();

        /// <summary>
        /// Создает тип доступа
        /// </summary>
        /// <returns>Экземпляр <see cref="IAccessType"/></returns>
        IAccessType GetAccessType();

        /// <summary>
        /// Производит сохранение всех сделанных изменений в базу данных
        /// </summary>
        /// <returns>Количество затронутых изменений</returns>
        int SaveChanges();

        Task<int> SaveChangesAsync();

        /// <summary>
        /// Создает новую транзакцию <see cref="ISecurityTransaction"/>
        /// </summary>
        /// <returns>Объект <see cref="ISecurityTransaction"/></returns>
        ISecurityTransaction BeginTransaction();

        /// <summary>
        /// Настройки для системы доступа
        /// </summary>
        ISecuritySettings Settings { get; }

        /// <summary>
        /// Текущее приложение
        /// </summary>
        IApplication CurrentApplication { get; }

        /// <summary>
        /// Создает приложение, если его нет
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="description"></param>
        void CreateAppIfNoExists(ISecurityApplicationInfo securityApplicationInfo);
    }
}
