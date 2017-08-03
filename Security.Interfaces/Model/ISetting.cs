using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Interfaces.Model
{
    /// <summary>
    /// Объект настройки для системы доступа
    /// </summary>
    public interface ISetting
    {
        /// <summary>
        /// Идентификатор в БД
        /// </summary>
        int IdSettings { get;}

        /// <summary>
        /// Ключ
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Значение
        /// </summary>
        string Value { get; set; }
        
        /// <summary>
        /// Описание
        /// </summary>
        string Description { get; set; }
    }
}
