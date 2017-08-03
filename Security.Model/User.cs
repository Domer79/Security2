using System;
using System.Collections.Generic;
using Security.Interfaces.Model;
using Tools.Extensions;

namespace Security.Model
{
    /// <summary>
    /// Объект "Пользователь"
    /// </summary>
    public class User : IUser
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public User()
        {
            Groups = new HashSet<Group>();
        }

        /// <summary>
        /// Дополнительный идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public int IdMember { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Статус - Активен/Заблокироан
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Строка данных, которая передаётся хеш-функции вместе с паролем
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Дата последней активности пользователя
        /// </summary>
        public DateTime? LastActivityDate { get; set; }

        /// <summary>
        /// Список групп пользователя
        /// </summary>
        public HashSet<Group> Groups { get; set; }

        /// <summary>
        /// Список групп пользователя
        /// </summary>
        IList<IGroup> IUser.Groups => new List<IGroup>(Groups);

        public static byte[] HashPassword(string password)
        {
            return password.GetMD5HashBytes();
        }
    }
}
