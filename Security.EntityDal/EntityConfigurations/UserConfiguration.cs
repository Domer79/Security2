using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Configuration;
using Security.Model;

namespace Security.EntityDal.EntityConfigurations
{
    /// <summary>
    /// Кофигурация сущности "Пользователь"
    /// </summary>
    public class UserConfiguration : BaseConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("UsersView");
            HasKey(e => e.IdMember);
            Property(e => e.IdMember).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.Login).IsUnicode().IsRequired();
            Property(e => e.FirstName).IsRequired().HasMaxLength(200).IsUnicode();
            Property(e => e.LastName).IsRequired().HasMaxLength(200).IsUnicode();
            Property(e => e.MiddleName).IsUnicode().HasMaxLength(200);
            Property(e => e.Email)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(400)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UQ_UserProfile_Email") { IsUnique = true }));
            Property(e => e.Status).IsRequired();
            Property(e => e.PasswordSalt)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(100)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UQ_UserProfile_PasswordSalt") { IsUnique = true}));

            Property(e => e.Id).IsOptional();

            MapToStoredProcedures(InsertConfiguration);
            MapToStoredProcedures(UpdateConfiguration);
            MapToStoredProcedures(DeleteConfiguration);

            HasMany(e => e.Groups).WithMany(e => e.Users).Map(cs =>
            {
                cs.MapLeftKey("idUser");
                cs.MapRightKey("idGroup");
                cs.ToTable("UserGroups");
            });
        }

        private void DeleteConfiguration(ModificationStoredProceduresConfiguration<User> p)
        {
            p.Delete(d => d.HasName("DeleteUser"));
        }

        private static void UpdateConfiguration(ModificationStoredProceduresConfiguration<User> p)
        {
            p.Update(u => u.HasName("UpdateUser"));
        }

        private void InsertConfiguration(ModificationStoredProceduresConfiguration<User> p)
        {
            p.Insert(i => i.HasName("AddUser").Result(r => r.IdMember, "idMember"));
        }
    }
}