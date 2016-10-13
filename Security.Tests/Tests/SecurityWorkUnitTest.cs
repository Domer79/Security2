﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.Tests.Interfaces;

namespace Security.Tests.Tests
{
    [TestClass]
    public class SecurityWorkUnitTest : ISecurityWorkUnitTest
    {
        /// <summary>
        /// Первичная настройка параметров. Настройка интерфейсов, первичная установка типов доступа
        /// </summary>
        public void SecurityInit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Тест. Добавление пользователя
        /// </summary>
        public void AddUserTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Тест. Добавление роли
        /// </summary>
        public void AddRoleTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Тест. Добавление объекта безопасности
        /// </summary>
        public void AddSecObjectTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Тест. Добавление разрешение на операцию Select для объекта на определенную роль
        /// </summary>
        public void AddGrantSelectForSecObject1ToRole1()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Тест. Предоставление роли пользователю
        /// </summary>
        public void AddRole1ToUser1Test()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Тест. Проверка входа
        /// </summary>
        public void LogInTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Тест. Проверка входа с неверным паролем
        /// </summary>
        public void LogInFailedTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Тест. Проверка доступа
        /// </summary>
        public void CheckAccessTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Тест. Проверка запрещенного доступа
        /// </summary>
        public void CheckAccessWrongTest()
        {
            throw new NotImplementedException();
        }
    }
}
