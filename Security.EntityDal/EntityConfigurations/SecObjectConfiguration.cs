using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Security.Model;

namespace Security.EntityDal.EntityConfigurations
{
    /// <summary>
    /// Кофигурация сущности "Объект безопасности"
    /// </summary>
    public class SecObjectConfiguration : BaseConfiguration<SecObject>
    {
        public SecObjectConfiguration()
        {
            ToTable("SecObjects");
            HasKey(e => e.IdSecObject);
            Property(e => e.IdSecObject).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.ObjectName)
                .IsUnicode(true)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UQ_SecObject_ObjectName") {IsUnique = true, Order = 1}));

            Property(e => e.IdApplication).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UQ_SecObject_ObjectName") { IsUnique = true, Order = 2}));

            Property(e => e.IdAccessType).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UQ_SecObject_ObjectName") { IsUnique = true, Order = 3}));

            HasMany(e => e.Grants).WithRequired(e => e.SecObject);
        }
    }
}