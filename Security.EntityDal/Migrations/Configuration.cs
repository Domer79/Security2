using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Security.Model;

namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Security.EntityDal.SecurityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Security.EntityDal.SecurityContext context)
        {
            var mainApplicationHostNameSetting = new Setting(){Name = "MainApplicationHostName", Description = "Хост главного приложения" };
            var mainAppPortSetting = new Setting() { Name = "MainApplicationPort", Description = "Порт главного приложения", Value = "80" };

            AddOrUpdate(mainApplicationHostNameSetting, e => e.Name == "MainApplicationHostName", context);
            AddOrUpdate(mainAppPortSetting, e => e.Name == "MainApplicationPort", context);
        }

        private static void AddOrUpdate<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> predicate, DbContext context) where TEntity: class
        {
            var dbSet = context.Set<TEntity>();
            if (!dbSet.Any(predicate))
                dbSet.Add(entity);
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }

            bool saveFiled;
            do
            {
                saveFiled = false;
                try
                {
                    context.SaveChanges();
                }
                catch (SecurityConfigurationSetException ex)
                {
                    var dbUpdateConcurrencyException = ex.InnerException as DbUpdateConcurrencyException;
                    if (dbUpdateConcurrencyException != null)
                    {
                        saveFiled = true;
                        dbUpdateConcurrencyException.Entries.Single().Reload();
                        continue;
                    }

                    throw;
                }
            } while (saveFiled);
        }
    }
}
