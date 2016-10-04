using Security.Model.Entities;

namespace Security.Model.EntityConfigurations
{
    public class MemberConfiguration : BaseConfiguration<Member>
    {
        public MemberConfiguration()
        {
            Map(e => e.ToTable("sec.Members"));
            Map<User>(u => u.ToTable("sec.Users"));
            Map<Group>(u => u.ToTable("sec.Groups"));

            Property(e => e.Name).IsUnicode(false);
            HasMany(e => e.Roles).WithMany(e => e.Members).Map(configuration =>
            {
                configuration.ToTable("MemberRoles");
                configuration.MapLeftKey("idMember");
                configuration.MapRightKey("idRole");
            });
        }
    }
}