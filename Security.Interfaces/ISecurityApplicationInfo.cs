using System.Diagnostics;

namespace Security.Interfaces
{
    /// <summary>
    /// Информация о приложении
    /// </summary>
    public interface ISecurityApplicationInfo
    {
        /// <summary>
        /// Имя приложения
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// Описание приложения
        /// </summary>
        string Description { get; set; }
    }
}