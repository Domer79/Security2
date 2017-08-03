using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Security.Exceptions;
using Security.Interfaces;
using Security.Interfaces.Collections;
using Security.Configurations;
using Security.Extensions;
using Security.Interfaces.Model;

namespace Security
{
    /// <summary>
    /// Представляет ядро системы
    /// </summary>
    public class CoreSecurity : ISecurity, IDisposable
    {
        private readonly ISecurityFactory _securityFactory;
        private IUserCollection _userCollection;
        private IGroupCollection _groupCollection;
        private ISecObjectCollection _secObjectCollection;
        private IRoleCollection _roleCollection;
        private IMemberCollection _memberCollection;
        private IGrantCollection _grantCollection;
        private IApplicationCollection _applicationCollection;

        public CoreSecurity()
            : this(Assembly.GetCallingAssembly().GetSecurityInfoFromAssembly().ApplicationName)
        {
        }

        public CoreSecurity(ISecurityApplicationInfo securityInfo)
            : this(securityInfo.ApplicationName)
        {
            
        }

        public CoreSecurity(string appName)
        {
            _securityFactory = Config.Get<ISecurityFactory>();
            _securityFactory.ApplicationName = appName;
            Tools = _securityFactory.CreateSecurityTools();
        }

        /// <summary>
        /// Коллекция пользователей
        /// </summary>
        public IUserCollection UserCollection
        {
            get
            {
                return _userCollection ?? (_userCollection = _securityFactory.CreateUserCollection());
            }
        }

        /// <summary>
        /// Коллекция групп пользователей
        /// </summary>
        public IGroupCollection GroupCollection
        {
            get { return _groupCollection ?? (_groupCollection = _securityFactory.CreateGroupCollection()); }
        }

        /// <summary>
        /// Коллекция объектов безопасности
        /// </summary>
        public ISecObjectCollection SecObjectCollection
        {
            get { return _secObjectCollection ?? (_secObjectCollection = _securityFactory.CreateSecObjectCollection()); }
        }

        /// <summary>
        /// Коллекция ролей
        /// </summary>
        public IRoleCollection RoleCollection
        {
            get { return _roleCollection ?? (_roleCollection = _securityFactory.CreateRoleCollection()); }
        }

        /// <summary>
        /// Коллекция участников безопасности. Объединяет в себе пользователей и группы пользователей
        /// </summary>
        public IMemberCollection MemberCollection
        {
            get { return _memberCollection ?? (_memberCollection = _securityFactory.CreateMemberCollection()); }
        }

        /// <summary>
        /// Коллекция разрешений
        /// </summary>
        public IGrantCollection GrantCollection
        {
            get { return _grantCollection ?? (_grantCollection = _securityFactory.CreateGrantCollection()); }
        }

        /// <summary>
        /// Коллекция разрешений со ссылкой на идентификатор приложения
        /// </summary>
        public IApplicationCollection ApplicationCollection
        {
            get
            {
                return _applicationCollection ?? (_applicationCollection = _securityFactory.CreateApplicationCollection());
            }
        }

        /// <summary>
        /// Проверка входа
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>Возвращает true, если проверка подлинности прошла успешно</returns>
        public bool LogIn(string login, string password)
        {
            return Tools.UserValidate(login, password);
        }

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns>Возвращает true, если доступ для пользователя разрешен</returns>
        public bool CheckAccess(string login, string secObjectName, Enum accessType)
        {
            return CheckAccess(login, secObjectName, accessType, null);
        }

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <param name="appName"></param>
        /// <returns>Возвращает true, если доступ для пользователя разрешен</returns>
        public bool CheckAccess(string login, string secObjectName, Enum accessType, string appName)
        {
            var accessTypeName = accessType.ToString();
            return Tools.CheckAccess(login, secObjectName, accessTypeName, appName ?? CurrentApplication.AppName);
        }

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessTypeName">Тип доступа</param>
        /// <returns>Возвращает true, если доступ для пользователя разрешен</returns>
        public bool CheckAccess(string login, string secObjectName, string accessTypeName)
        {
            return CheckAccess(login, secObjectName, accessTypeName, null);
        }

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessTypeName">Тип доступа</param>
        /// <param name="appName"></param>
        /// <returns>Возвращает true, если доступ для пользователя разрешен</returns>
        public bool CheckAccess(string login, string secObjectName, string accessTypeName, string appName)
        {
            return Tools.CheckAccess(login, secObjectName, accessTypeName, appName ?? CurrentApplication.AppName);
        }

        /// <summary>
        /// Возвращает все объекты безопасности для указанного типа доступа и текущего приложения
        /// </summary>
        /// <param name="accessType"></param>
        /// <returns></returns>
        public IEnumerable<string> GetAllowAllSecurityObjects(string accessType)
        {
            return Tools.GetAllowAllSecurityObjects(accessType, CurrentApplication.AppName);
        }

        /// <summary>
        /// Возвращает все доступные для пользователя объекты безопасности для указанного типа доступа и текущего приложения
        /// </summary>
        /// <param name="login"></param>
        /// <param name="accessType"></param>
        /// <returns></returns>
        public IEnumerable<string> GetAllowSecurityObjects(string login, string accessType)
        {
            return Tools.GetAllowSecurityObjects(login, accessType, CurrentApplication.AppName);
        }

        /// <summary>
        /// Дополнительные инструменты работы с данными
        /// </summary>
        public ISecurityTools Tools { get; }

        /// <summary>
        /// Создает новую транзакцию для работы с БД
        /// </summary>
        /// <returns>Возвращает вновь созданную транзакцию</returns>
        public ISecurityTransaction BeginTransaction()
        {
            return _securityFactory.BeginTransaction();
        }

        /// <summary>
        /// Возвращает коллекцию типов доступа для текущего приложения
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IAccessType> GetAccessTypes()
        {
            return _securityFactory.CreateAccessTypeCollection();
        }

        /// <summary>
        /// Коллекция и инструментарий работы с настройками
        /// </summary>
        public ISecuritySettings Settings => _securityFactory.Settings;

        /// <summary>
        /// Производит сохранение всех изменений в настройках системы безопасности
        /// </summary>
        /// <returns>Возвращает количество затронутых строк</returns>
        public int SaveChanges()
        {
            return _securityFactory.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _securityFactory.SaveChangesAsync();
        }

        /// <summary>
        /// Текущее приложение
        /// </summary>
        public IApplication CurrentApplication
        {
            get
            {
                return _securityFactory.CurrentApplication;
            }
        }

        /// <summary>
        /// Уничтожает ресурсы занимаемые ядром системы
        /// </summary>
        public void Dispose()
        {
//            _securityFactory.Dispose();
        }
    }
}
