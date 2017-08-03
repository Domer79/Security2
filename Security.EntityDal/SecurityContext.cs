using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Security.EntityDal.Base;
using Security.EntityDal.EntityConfigurations;
using Security.EntityDal.Migrations;
using Security.Model;
using Tools.Extensions;
using System.Data.SqlClient;

namespace Security.EntityDal
{
    public class SecurityContext : RepositoryDataContext
    {
        private readonly bool _databaseExists;

        public SecurityContext()
            : base(Infrastructure.Tools.ConnectionString)
        {
            if (_databaseExists || (_databaseExists = Database.Exists()))
                Database.SetInitializer(new ContainerInitializer());
            else
            {
                Database.SetInitializer(new CreateDatabaseIfNotExists<SecurityContext>());
            }

            Database.Initialize(false);
        }

        private class ContainerInitializer : MigrateDatabaseToLatestVersion<SecurityContext, Configuration>
        {
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<AccessType> AccessTypes { get; set; }
        public virtual DbSet<SecObject> SecObjects { get; set; }
        public virtual DbSet<Grant> Grants { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("sec");

            modelBuilder.Configurations.Add(new SecObjectConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new AccessTypeConfiguration());
            modelBuilder.Configurations.Add(new GrantConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new MemberConfiguration());
            modelBuilder.Configurations.Add(new LogConfiguration());
            modelBuilder.Configurations.Add(new SettingConfiguration());
            modelBuilder.Configurations.Add(new ApplicationConfiguration());
        }

        /// <summary>
        /// Производит сохранение всех изменений контекста в базу данных
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var entityValidationResults = GetValidationErrors().Select(er => new SecurityEntityValidationResult(er));
                var sb = new StringBuilder();
                foreach (var validationResult in entityValidationResults)
                {
                    sb.AppendLine(validationResult.ToString());
                }
                throw new SecurityConfigurationSetException(sb.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new SecurityConfigurationSetException(ex.GetErrorMessage(), ex);
            }
        }

        /// <summary>
        /// Производит асинхронное сохранение всех изменений контекста в базу данных
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException ex)
            {
                var entityValidationResults = GetValidationErrors().Select(er => new SecurityEntityValidationResult(er));
                var sb = new StringBuilder();
                foreach (var validationResult in entityValidationResults)
                {
                    sb.AppendLine(validationResult.ToString());
                }
                throw new SecurityConfigurationSetException(sb.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new SecurityConfigurationSetException(ex.GetErrorMessage(), ex);
            }
        }

        /// <summary>
        /// Возвращает все объекты безопасности для указанного типа доступа и приложения
        /// </summary>
        /// <param name="accessType">Тип доступа</param>
        /// <param name="appName">Имя приложения</param>
        /// <returns></returns>
        public IQueryable<string> GetSecurityObjectsForUserByAccessType(string accessType, string appName)
        {
            return GetSecurityObjectsForUserByAccessType(null, accessType, appName, true);
        }

        /// <summary>
        /// Возвращает разрешенные для пользователя объекты безопасности для указанного типа доступа и приложения
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="accessType">Тип доступа</param>
        /// <param name="appName">Имя приложения</param>
        /// <param name="allowAll">Если установлено true, возвращает все объекты безопасности для указанного типа доступа и приложения, иначе только те, которые разрешены для пользователя</param>
        /// <returns></returns>
        public IQueryable<string> GetSecurityObjectsForUserByAccessType(string login, string accessType, string appName, bool allowAll)
        {
            var loginParameter = new SqlParameter("login", System.Data.SqlDbType.NVarChar);
            loginParameter.Value = (object) login ?? DBNull.Value;
            var appNameParameter = new SqlParameter("appName", System.Data.SqlDbType.NVarChar);
            appNameParameter.Value = appName;
            var accessTypeParameter = new SqlParameter("accessType", System.Data.SqlDbType.NVarChar);
            accessTypeParameter.Value = accessType;
            var allowAllParameter = new SqlParameter("allowAll", SqlDbType.Bit);
            allowAllParameter.Value = allowAll;

            return Database.SqlQuery<string>("Exec sec.GetSecurityObjects @login, @appName, @accessType, @allowAll", loginParameter, appNameParameter, accessTypeParameter, allowAllParameter).AsQueryable();
        }
    }
}
