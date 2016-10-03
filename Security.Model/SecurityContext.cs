using System.Data.Entity;
using Security.Model.Base;
using Security.Model.Entities;
using Security.Model.EntityConfigurations;

namespace Security.Model
{
    public abstract class SecurityContext : RepositoryDataContext
    {
        protected SecurityContext()
            : base(Infrastructure.Tools.ConnectionString)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<AccessType> AccessTypes { get; set; }
        public virtual DbSet<SecObject> SecObjects { get; set; }
        public virtual DbSet<Grant> Grants { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<RoleOfMember> RoleOfMembers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroupsDetail> UserGroupsDetails { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new AccessTypeConfiguration());
            modelBuilder.Configurations.Add(new SecObjectConfiguration());
            modelBuilder.Configurations.Add(new GrantConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new RoleOfMemberConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserGroupsDetailConfiguration());
            modelBuilder.Configurations.Add(new MemberConfiguration());
        }
    }
}
