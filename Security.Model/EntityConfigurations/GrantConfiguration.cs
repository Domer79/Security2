using Security.Model.Entities;

namespace Security.Model.EntityConfigurations
{
    public class GrantConfiguration : BaseConfiguration<Grant>
    {
        public GrantConfiguration()
        {
            MapToStoredProcedures(p => p.Insert(i => i.HasName("sec.AddGrant")));
            MapToStoredProcedures(p => p.Update(u => u.HasName("sec.UpdateGrant")));
            MapToStoredProcedures(p => p.Delete(d => d.HasName("sec.DeleteGrant")));
        }
    }
}