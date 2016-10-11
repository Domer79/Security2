using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using Itis.ProxyDataPlatform.Base;

namespace Itis.ProxyDataPlatform.DatabaseContext
{
    public class BaseContainer : DbContext
    {
        public BaseContainer(string nameOrConnectionString) 
            : base(nameOrConnectionString)
        {
        }

        public override int SaveChanges()
        {
            var context = ((IObjectContextAdapter) this).ObjectContext;
            context.DetectChanges();

            //Find all Entities that are Added/Modified that inherit from my Entity
            var objectStateEntries =
                context.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified)
                    .Where(e => e.IsRelationship == false && e.Entity is Entity);

            var currentTime = DateTime.UtcNow;

            foreach (var entry in objectStateEntries)
            {
                var entityBase = entry.Entity as Entity;
                if (entityBase == null) continue;

                if (entry.State == EntityState.Added)
                    entityBase.DateCreated = currentTime;

                entityBase.DateUpdated = currentTime;
            }

            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw new EntityValidationExceptionWrap(e);
            }
        }
    }

    /// <summary>
    /// Обертка над <see cref="DbEntityValidationException"/> чтобы в свойстве <see cref="Itis.St.Suno.Model.EntityValidationExceptionWrap.Message"/> 
    /// содержалась подробная ошибка с указанием свойства на котором не пройдена валидация и его значение
    /// </summary>
    public class EntityValidationExceptionWrap : Exception
    {
        public EntityValidationExceptionWrap(DbEntityValidationException innerException) :
            base(null, innerException)
        {
        }

        public override string Message
        {
            get
            {
                var innerException = InnerException as DbEntityValidationException;
                if (innerException != null)
                {
                    var sb = new StringBuilder();

                    sb.AppendLine();
                    foreach (var eve in innerException.EntityValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sb.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                ve.PropertyName,
                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                ve.ErrorMessage));
                        }
                    }
                    sb.AppendLine();

                    return sb.ToString();
                }

                return base.Message;
            }
        }
    }
}
