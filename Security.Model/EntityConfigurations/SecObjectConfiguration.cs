using Security.Model.Entities;

namespace Security.Model.EntityConfigurations
{
    public class SecObjectConfiguration : BaseConfiguration<SecObject>
    {
        public SecObjectConfiguration()
        {
            HasMany(e => e.Grants).WithRequired(e => e.SecObject);
        }
    }
}