namespace Security.Interfaces.Model
{
    /// <summary>
    /// Объект разрешения
    /// </summary>
    public interface IGrant
    {
        /// <summary>
        /// Идентификатор объекта безопасности
        /// </summary>
        int IdSecObject { get; }

        /// <summary>
        /// Идентификатор роли
        /// </summary>
        int IdRole { get; }

        /// <summary>
        /// Идентификатор типа доступа
        /// </summary>
        int IdAccessType { get; }

        /// <summary>
        /// Роль
        /// </summary>
        IRole Role { get; set; }

        /// <summary>
        /// Тип доступа
        /// </summary>
        IAccessType AccessType { get; set; }

        /// <summary>
        /// Объект безопасности
        /// </summary>
        ISecObject SecObject { get; set; }
    }
}