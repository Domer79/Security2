using System.Data.Entity.ModelConfiguration;

namespace Security.EntityDal.EntityConfigurations
{
    /// <summary>
    /// Базовая конфигурация сущности
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseConfiguration<TEntity> : EntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        public const string Schema = "sec";
    }
}