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
        void AddGrantSelectForSecObject1ToRole1();

        /// <summary>
        /// Тест. Предоставление роли пользователю
        /// </summary>
        void AddRole1ToUser1Test();

        /// <summary>
        /// Тест. Проверка входа
        /// </summary>
        void LogInTest();

        /// <summary>
        /// Тест. Проверка входа с неверным паролем
        /// </summary>
        void LogInFailedTest();

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