using System.Data.Entity.ModelConfiguration.Configuration;
using Security.Model.Entities;

namespace Security.Model.EntityConfigurations
{
    public class GroupConfiguration : BaseConfiguration<Group>
    {
        public GroupConfiguration()
        {
            Property(e => e.Description)
                .IsUnicode(false);

            MapToStoredProcedures(InsertConfiguration);
            MapToStoredProcedures(UpdateConfiguration);
            MapToStoredProcedures(DeleteConfiguration);
        }
        private void DeleteConfiguration(ModificationStoredProceduresConfiguration<Group> p)
        {
            p.Delete(d => d.HasName("sec.DeleteGroup"));
        }

        private static void UpdateConfiguration(ModificationStoredProceduresConfiguration<Group> p)
        {
            p.Update(u => u.HasName("sec.UpdateGroup"));
        }

        public void InsertConfiguration(ModificationStoredProceduresConfiguration<Group> p)
        {
            p.Insert(i => i.HasName("sec.AddGroup").Result(r => r.IdMember, "idMember"));
        }
    }
}