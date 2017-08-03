using System.ComponentModel.DataAnnotations.Schema;
using Security.Model;

namespace Security.EntityDal.EntityConfigurations
{
    /// <summary>
    /// Конфигурация сущности "Разрешение"
    /// </summary>
    public class GrantConfiguration : BaseConfiguration<Grant>
    {
        public GrantConfiguration()
        {
            ToTable("Grants");
            HasKey(e => new { e.IdSecObject, e.IdRole, e.IdAccessType});

            Property(e => e.IdSecObject).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnOrder(1);
            Property(e => e.IdRole).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnOrder(2);
            Property(e => e.IdAccessType).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).HasColumnOrder(3);

            MapToStoredProcedures(p => p.Insert(i => i.HasName("AddGrant")));
            MapToStoredProcedures(p => p.Update(u => u.HasName("UpdateGrant")));
            MapToStoredProcedures(p => p.Delete(d => d.HasName("DeleteGrant")));
        }
    }
} 