using System.Data.Entity.Infrastructure;

namespace Security.EntityDal
{
    internal class SecurityEntityEntry
    {
        public SecurityEntityEntry(DbEntityEntry entry)
        {
            Entity = entry.Entity.ToString();
            State = entry.State.ToString();
        }

        public string Entity { get; set; }
        public string State { get; set; }

        public override string ToString()
        {
            return $"Entity: {Entity}; State: {State}";
        }
    }
}