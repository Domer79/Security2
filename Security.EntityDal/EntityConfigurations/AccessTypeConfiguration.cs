using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Security.Model;

namespace Security.EntityDal.EntityConfigurations
{
    /// <summary>
    /// Конфигурация для сущности "Тип доступа"
    /// </summary>
    public class AccessTypeConfiguration : BaseConfiguration<AccessType>
    {
        public AccessTypeConfiguration()
        {
            ToTable("AccessTypes");
            HasKey(e => e.IdAccessType);
            Property(e => e.IdAccessType).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UQ_AccessType_AccessName") { IsUnique = true }));

            HasMany(e => e.SecObjects).WithRequired(e => e.AccessType);
            HasMany(e => e.Grants).WithRequired(e => e.AccessType);
        }
    }
}