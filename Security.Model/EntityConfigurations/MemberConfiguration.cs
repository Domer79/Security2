using Security.Model.Entities;

namespace Security.Model.EntityConfigurations
{
    public class MemberConfiguration : BaseConfiguration<Member>
    {
        public MemberConfiguration()
        {
            Property(e => e.Name)
                .IsUnicode(false);
        }
    }
}