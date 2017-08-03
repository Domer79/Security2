using System.Data.Entity;
using Security.EntityDal;
using Security.Interfaces;

namespace Security.EntityFramework
{
    /// <summary>
    /// Класс, представляющий стандартную реализацию объекта транзакции
    /// </summary>
    public class SecurityTransaction : ISecurityTransaction
    {
        private readonly DbContextTransaction _transaction;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контект <see cref="SecurityContext"/></param>
        public SecurityTransaction(SecurityContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _transaction.Dispose();
        }

        /// <summary>
        /// Фиксирует изменения в базе данных
        /// </summary>
        public void Commit()
        {
            _transaction.Commit();
        }

        /// <summary>
        /// Отменяет сделанные изменения
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}