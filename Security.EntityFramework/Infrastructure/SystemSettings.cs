
namespace Security.EntityFramework.Infrastructure
{
    /// <summary>
    /// Настройки системы управления доступом 
    /// </summary>
    public enum SystemSettings
    {
        /// <summary>
        /// Наименование хоста сайта главного приложения, на которое строится ссылка в панели администрирования
        /// </summary>
        MainApplicationHostName = 100,

        /// <summary>
        /// Порт хоста сайта главного приложения
        /// </summary>
        MainApplicationPort = 101
    }
}
