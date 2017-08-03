using System;

namespace Security.Interfaces.Model
{
    /// <summary>
    /// Запись в журнале
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Идентификатор лога
        /// </summary>
        int IdLog { get; set; }

        /// <summary>
        /// Сообщение для сохранения в лог
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Дата возникновения записи
        /// </summary>
        DateTime DateCreated { get; set; }
    }
}