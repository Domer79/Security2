using System;

namespace Itis.ProxyDataPlatform.Interfaces
{
    /// <summary>
    /// Базовая сущность модели
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Уникальный идентификатор сущности
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Дата создания экземпляра
        /// </summary>
        DateTime DateCreated { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        DateTime? DateUpdated { get; set; }
    }
}