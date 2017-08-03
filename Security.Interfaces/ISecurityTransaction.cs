using System;

namespace Security.Interfaces
{
    /// <summary>
    /// Интерфейс, представляющий стандартную реализацию объекта транзакции
    /// </summary>
    public interface ISecurityTransaction : IDisposable
    {
        /// <summary>
        /// Фиксирует изменения в базе данных
        /// </summary>
        void Commit();

        /// <summary>
        /// Отменяет сделанные изменения
        /// </summary>
        void Rollback();
    }
}