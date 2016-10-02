using Security.Model.Entities;

namespace Security.Model.EntityConfigurations
{
    internal class RoleConfiguration : BaseConfiguration<Role>
    {
        public RoleConfiguration()
        {
            Property(e => e.Name)
                .IsUnicode(false);

            HasMany(e => e.Grants).WithRequired(e => e.Role);
            HasMany(e => e.RoleOfMembers).WithRequired(e => e.Role);
        }
    }
}