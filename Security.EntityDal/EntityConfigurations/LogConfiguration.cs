using System.ComponentModel.DataAnnotations.Schema;
using Security.Model;

namespace Security.EntityDal.EntityConfigurations
{
    /// <summary>
    /// Конфигурация сущности "Лог"
    /// </summary>
    public class LogConfiguration : BaseConfiguration<Log>
    {
        public LogConfiguration()
        {
            ToTable("Logs");
            HasKey(e => e.IdLog);
            Property(e => e.IdLog).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.Message)
                .IsRequired()
                .IsUnicode();
        }
    }
}
