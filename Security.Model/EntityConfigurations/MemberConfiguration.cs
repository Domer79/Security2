using System.Data.Entity.ModelConfiguration.Configuration;
using Security.Model.Entities;

namespace Security.Model.EntityConfigurations
{
    public class MemberConfiguration : BaseConfiguration<Member>
    {
        public MemberConfiguration()
        {
            Property(e => e.Name).IsUnicode(false);
            HasMany(e => e.Roles).WithMany(e => e.Members).Map(configuration =>
            {
                configuration.ToTable("MemberRoles", "sec");
                configuration.MapLeftKey("idMember");
                configuration.MapRightKey("idRole");
            });
        }
    }
}