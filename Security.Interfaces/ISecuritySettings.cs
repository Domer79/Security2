using System.Collections.Generic;
using Security.Interfaces.Model;

namespace Security.Interfaces
{
    /// <summary>
    /// Интерфейс настроечных параметров для системы доступа
    /// </summary>
    public interface ISecuritySettings
    {
        /// <summary>
        /// Возвращает значение переданного ключа <see cref="key"/>
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Значение переданного ключа</returns>
        string this[string key] { get; set; }

        /// <summary>
        /// Возвращает значение переданного ключа <see cref="key"/>
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="key">Ключ</param>
        /// <returns>Значение типа <see cref="T"/></returns>
        T GetValue<T>(string key);

        /// <summary>
        /// Устанавливает значения для ключа <see cref="key"/>
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        void SetValue<T>(string key, T value);
        /// <summary>
        /// Возвращает коллекцю системных настроек
        /// </summary>        
        IEnumerable<ISetting> GetSystemSettings();
    }
}