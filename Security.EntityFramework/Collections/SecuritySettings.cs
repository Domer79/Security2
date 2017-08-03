using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Security.EntityDal;
using Security.Interfaces;
using Security.Interfaces.Model;
using Security.Model;
using Security.EntityFramework.Infrastructure;

namespace Security.EntityFramework.Collections
{

    internal class SecuritySettings : ISecuritySettings
    {
        private readonly SecurityContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public SecuritySettings(SecurityContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возвращает значение переданного ключа <see cref="key"/>
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Значение переданного ключа</returns>
        public string this[string key]
        {
            get { return _context.Settings.First(e => e.Name.Equals(key, StringComparison.OrdinalIgnoreCase)).Value; }
            set
            {
                var setting = _context.Settings.FirstOrDefault(e => e.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
                if (setting != null)
                {
                    setting.Value = value;
                    _context.SaveChanges();
                    return;
                }

                setting = new Setting {Name = key, Value = value};
                _context.Settings.Add(setting);
                _context.SaveChanges();
            }
        }
        /// <summary>
        /// Возвращает коллекцию системных настроек <see cref="Security.EntityFramework.Infrastructure.SystemSettings"/>>
        /// </summary>        
        public IEnumerable<ISetting> GetSystemSettings()
        {
            var settingKeys = new List<string>();
            foreach (SystemSettings setting in Enum.GetValues(typeof(SystemSettings)))
            {
                settingKeys.Add(setting.ToString());
            }

            return _context.Settings.Where(s => settingKeys.Contains(s.Name));
        }

        /// <summary>
        /// Возвращает значение переданного ключа <see cref="key"/>
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="key">Ключ</param>
        /// <returns>Значение типа <see cref="T"/></returns>
        public T GetValue<T>(string key)
        {
            var value = this[key];
            return (T) Convert.ChangeType(value, typeof (T));
        }

        /// <summary>
        /// Устанавливает значения для ключа <see cref="key"/>
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        public void SetValue<T>(string key, T value)
        {
            this[key] = value.ToString();
        }
    }
}
