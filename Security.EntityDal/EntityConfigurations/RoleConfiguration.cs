using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Security.Model;

namespace Security.EntityDal.EntityConfigurations
{
    /// <summary>
    /// Кофигурация сущности "Роль"
    /// </summary>
    internal class RoleConfiguration : BaseConfiguration<Role>
    {
        public RoleConfiguration()
        {
            ToTable("Roles");
            HasKey(e => e.IdRole);
            Property(e => e.IdRole).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Name)
                .IsUnicode()
                .IsRequired()
                .HasMaxLength(300)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UQ_Role_Name") {IsUnique = true}));

            HasMany(e => e.Grants).WithRequired(e => e.Role);
            HasMany(e => e.Members).WithMany(e => e.Roles);
        }
    }
}