using System;
using Security.Interfaces.Model;

namespace Security.Model
{
    /// <summary>
    /// Объект настройки для системы доступа
    /// </summary>

    public class Setting : ISetting
    {      
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        public int IdSettings { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}