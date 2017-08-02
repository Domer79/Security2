using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Configuration;
using Security.Model;

namespace Security.EntityDal.EntityConfigurations
{
    /// <summary>
    /// Конфигурация ссущности "Группа пользователей"
    /// </summary>
    public class GroupConfiguration : BaseConfiguration<Group>
    {
        public GroupConfiguration()
        {
            ToTable("GroupsView");
            HasKey(e => e.IdMember);
            Property(e => e.IdMember).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(200);

            Property(e => e.Description).IsUnicode();
            Property(e => e.Id).IsOptional();

            MapToStoredProcedures(InsertConfiguration);
            MapToStoredProcedures(UpdateConfiguration);
            MapToStoredProcedures(DeleteConfiguration);
        }
        private void DeleteConfiguration(ModificationStoredProceduresConfiguration<Group> p)
        {
            p.Delete(d => d.HasName("DeleteGroup"));
        }

        private static void UpdateConfiguration(ModificationStoredProceduresConfiguration<Group> p)
        {
            p.Update(u => u.HasName("UpdateGroup"));
        }

        public void InsertConfiguration(ModificationStoredProceduresConfiguration<Group> p)
        {
            p.Insert(i => i.HasName("AddGroup").Result(r => r.IdMember, "idMember"));
        }
    }
} 