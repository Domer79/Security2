using System.Data.Entity.ModelConfiguration.Configuration;
using Security.Model.Entities;

namespace Security.Model.EntityConfigurations
{
    public class UserConfiguration : BaseConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(e => e.Login)
                .IsUnicode(false);

            Property(e => e.DisplayName)
                .IsUnicode(false);

            Property(e => e.Email)
                .IsUnicode(false);

            MapToStoredProcedures(InsertConfiguration);
            MapToStoredProcedures(UpdateConfiguration);
            MapToStoredProcedures(DeleteConfiguration);
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
            p.Insert(i => i.HasName("sec.AddUser").Result(r => r.IdUser, "idMember"));
        }
    }
}