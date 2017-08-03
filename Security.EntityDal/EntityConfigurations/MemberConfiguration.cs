using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Security.Model;

namespace Security.EntityDal.EntityConfigurations
{
    /// <summary>
    /// Кофигурация сущности "Участник безопасности"
    /// </summary>
    public class MemberConfiguration : BaseConfiguration<Member>
    {
        public MemberConfiguration()
        {
            ToTable("Members");
            HasKey(e => e.IdMember);
            Property(e => e.IdMember).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.Name)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("UQ_Member_Name") {IsUnique = true}))
                .IsUnicode();

            HasMany(e => e.Roles).WithMany(e => e.Members).Map(configuration =>
            {
                configuration.ToTable("MemberRoles");
                configuration.MapLeftKey("idMember");
                configuration.MapRightKey("idRole");
            });
        }
    }
}