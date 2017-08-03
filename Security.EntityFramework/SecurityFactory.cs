using System;
using System.Linq;
using System.Threading.Tasks;
using Security.EntityDal;
using Security.EntityFramework.Collections;
using Security.EntityFramework.Exceptions;
using Security.Interfaces;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;
using Security.Model;

namespace Security.EntityFramework
{
    /// <summary>
    /// Фабрика, для предоставления всех необходимых компонентов для работы ядра <see cref="ISecurity"/>
    /// </summary>
    public class SecurityFactory : ISecurityFactory
    {
        private readonly SecurityContext _securityContext;

        public SecurityFactory(SecurityContext securityContext)
        {
            _securityContext = securityContext;
        }

        /// <summary>
        /// Наименование приложения
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Создает коллекцию типов доступа
        /// </summary>
        /// <returns>Коллекцию типов доступа</returns>
        public IAccessTypeCollection CreateAccessTypeCollection()
        {
            return new AccessTypeCollection(_securityContext, CurrentApplication);
        }

        /// <summary>
        /// Создает коллекцию разрешений
        /// </summary>
        /// <returns>Коллекцию разрешений</returns>
        public IGrantCollection CreateGrantCollection()
        {
            return new GrantCollection(_securityContext);
        }

        /// <summary>
        /// Создает коллекцию групп пользователей
        /// </summary>
        /// <returns>Коллекцию групп пользователей</returns>
        public IGroupCollection CreateGroupCollection()
        {
            return new GroupCollection(_securityContext);
        }

        /// <summary>
        /// Создает коллекцию участников безопасности
        /// </summary>
        /// <returns>Коллекцию участников безопасности</returns>
        public IMemberCollection CreateMemberCollection()
        {
            return new MemberCollection(_securityContext);
        }

        /// <summary>
        /// Создает коллекцию ролей
        /// </summary>
        /// <returns>Коллекцию ролей</returns>
        public IRoleCollection CreateRoleCollection()
        {
            return new RoleCollection(_securityContext, CurrentApplication);
        }

        /// <summary>
        /// Создает коллекцию объектов безопасности
        /// </summary>
        /// <returns>Коллекцию объектов безопасности</returns>
        public ISecObjectCollection CreateSecObjectCollection()
        {
            return new SecObjectCollection(_securityContext, CurrentApplication);
        }

        /// <summary>
        /// Создает коллекцию пользователей
        /// </summary>
        /// <returns>Коллекцию пользователей</returns>
        public IUserCollection CreateUserCollection()
        {
            return new UserCollection(_securityContext);
        }

        /// <summary>
        /// Создает коллекцию приложений
        /// </summary>
        /// <returns>Коллекцию приложений</returns>
        public IApplicationCollection CreateApplicationCollection()
        {
            return new ApplicationCollection(_securityContext);
        }

        /// <summary>
        /// Создает эзкемпляр класса <see cref="ISecurityTools"/>
        /// </summary>
        /// <returns>Объект <see cref="ISecurityTools"/></returns>
        public ISecurityTools CreateSecurityTools()
        {
            return new SecurityTools(_securityContext);
        }

        /// <summary>
        /// Создает тип доступа
        /// </summary>
        /// <returns>Экземпляр <see cref="IAccessType"/></returns>
        public IAccessType GetAccessType()
        {
            return new AccessType() {Application = (Application) CurrentApplication};
        }
        
        /// <summary>
        /// Производит сохранение всех сделанных изменений в базу данных
        /// </summary>
        /// <returns>Количество затронутых изменений</returns>
        public int SaveChanges()
        {
            return _securityContext.SaveChanges();
        }

        /// <summary>
        /// Производит сохранение всех сделанных изменений в базу данных в ассинхронном режиме
        /// </summary>
        /// <returns>Количество затронутых изменений</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _securityContext.SaveChangesAsync();
        }

        /// <summary>
        /// Создает новую транзакцию <see cref="ISecurityTransaction"/>
        /// </summary>
        /// <returns>Объект <see cref="ISecurityTransaction"/></returns>
        public ISecurityTransaction BeginTransaction()
        {
            return new SecurityTransaction(_securityContext);
        }

        /// <summary>
        /// Настройки для системы доступа
        /// </summary>
        public ISecuritySettings Settings => new SecuritySettings(_securityContext);

        /// <summary>
        /// Текущее приложение
        /// </summary>
        /// <exception cref="ApplicationMissingException"></exception>
        public IApplication CurrentApplication
        {
            get
            {
                try
                {
                    return _securityContext.Applications.First(e => e.AppName == ApplicationName);
                }
                catch (Exception e)
                {
                    throw new ApplicationMissingException($"Приложения {ApplicationName} не существует.", e);
                }
            }
        }

        /// <summary>
        /// Создает приложение, если его нет
        /// </summary>
        /// <param name="securityApplicationInfo"></param>
        public void CreateAppIfNoExists(ISecurityApplicationInfo securityApplicationInfo)
        {
            CreateAppIfNoExists(securityApplicationInfo.ApplicationName, securityApplicationInfo.Description);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _securityContext.Dispose();
        }

        private void CreateAppIfNoExists(string applicationName, string description)
        {
            var collection = CreateApplicationCollection();
            var appname = applicationName ?? ApplicationName;
            var application = collection.SingleOrDefault(e => e.AppName == applicationName);
            if (application != null)
            {
                application.Description = description;
            }
            else
            {
                collection.Add(new Application() { AppName = appname, Description = description });
            }
                
            collection.SaveChanges();
        }
    }
}