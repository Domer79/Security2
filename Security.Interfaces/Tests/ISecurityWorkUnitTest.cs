namespace Security.Interfaces.Tests
{
    public interface ISecurityWorkUnitTest
    {
        /// <summary>
        /// Первичная настройка параметров. Настройка интерфейсов, первичная установка типов доступа
        /// </summary>
        void SecurityInit();

        /// <summary>
        /// Тест. Добавление пользователя
        /// </summary>
        void AddUserTest();

        /// <summary>
        /// Тест. Добавление роли
        /// </summary>
        void AddRoleTest();

        /// <summary>
        /// Тест. Добавление объекта безопасности
        /// </summary>
        void AddSecObjectTest();

        /// <summary>
        /// Тест. Добавление разрешение на операцию Select для объекта на определенную роль
        /// </summary>
        void AddGrant_CheckGrantAfterInsert_ExpectedRole1SecObject1Exec();

        /// <summary>
        /// Тест. Предоставление роли пользователю
        /// </summary>
        void AddRolesToMember_CheckRelationShip_ExpectedValue(string user, string[] roles, string app);

        /// <summary>
        /// Тест добавления пользователей в группу. Проверка наличия добавленных пользователей в группе
        /// </summary>
        /// <param name="group"></param>
        /// <param name="users"></param>
        void AddUsersToGroup_CheckRelationship(string group, string[] users);

        /// <summary>
        /// Тест. Проверка входа
        /// </summary>
        void LogIn_CheckAuthentication_ExpectedTrue(string user, string password);

        /// <summary>
        /// Тест. Проверка входа с неверным паролем
        /// </summary>
        void LogIn_CheckAuthentication_ExpectedFalse(string user, string password);

        /// <summary>
        /// Тест. Проверка доступа
        /// </summary>
        void CheckAccessTest();

        /// <summary>
        /// Тест. Проверка запрещенного доступа
        /// </summary>
        void CheckAccessWrongTest();
    }
}