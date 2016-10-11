using System;
using System.Data.Entity;

namespace Itis.ProxyDataPlatform
{
    public class SunoDbContextTransaction :IDisposable
    {
        private readonly DbContextTransaction _transaction;
        public SunoDbContextTransaction(DbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
        }
    }
}