using System;
using Security.Interfaces.Model;

namespace Security.Model
{
    /// <summary>
    /// Запись в журнале
    /// </summary>
    public class Log : ILog
    {
        public Log()
        {
            DateCreated = DateTime.UtcNow;
        }

        /// <summary>
        /// Идентификатор лога
        /// </summary>
        public int IdLog { get; set; }

        /// <summary>
        /// Сообщение для сохранения в лог
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Дата возникновения записи
        /// </summary>
        public DateTime DateCreated { get; set; }
    }
}
