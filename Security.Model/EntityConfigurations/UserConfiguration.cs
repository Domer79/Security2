using System.Data.Entity.ModelConfiguration.Configuration;
using Security.Model.Entities;

namespace Security.Model.EntityConfigurations
{
    public class UserConfiguration : BaseConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(e => e.Login).IsUnicode(false);

            MapToStoredProcedures(InsertConfiguration);
            MapToStoredProcedures(UpdateConfiguration);
            MapToStoredProcedures(DeleteConfiguration);

            HasMany(e => e.Groups).WithMany(e => e.Users).Map(cs =>
            {
                cs.MapLeftKey("idUser");
                cs.MapRightKey("idGroup");
                cs.ToTable("UserGroups", "sec");
            });
        }

        private void DeleteConfiguration(ModificationStoredProceduresConfiguration<User> p)
        {
            p.Delete(d => d.HasName("sec.DeleteUser"));
        }

        private static void UpdateConfiguration(ModificationStoredProceduresConfiguration<User> p)
        {
            p.Update(u => u.HasName("sec.UpdateUser"));
        }

        private void InsertConfiguration(ModificationStoredProceduresConfiguration<User> p)
        {
            p.Insert(i => i.HasName("sec.AddUser").Result(r => r.IdMember, "idMember"));
        }
    }
}