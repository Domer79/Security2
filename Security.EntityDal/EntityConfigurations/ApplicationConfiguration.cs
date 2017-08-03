using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Model;

namespace Security.EntityDal.EntityConfigurations
{
    public class ApplicationConfiguration : BaseConfiguration<Application>
    {
        public ApplicationConfiguration()
        {
            ToTable("Applications");

            HasKey(e => e.IdApplication);
            Property(e => e.IdApplication).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.AppName)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasMaxLength(200)
                .IsUnicode()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UQ_Applications_Name") {IsUnique = true}))
                ;
            Property(e => e.Description).IsUnicode();

            HasMany(e => e.Roles).WithRequired(e => e.Application);
            HasMany(e => e.SecObjects).WithRequired(e => e.Application);
            HasMany(e => e.AccessTypes).WithRequired(e => e.Application);

            MapToStoredProcedures(configuration => configuration.Insert(d => d.HasName("AddApp").Result(r => r.IdApplication, "idApplication")));
            MapToStoredProcedures(configuration => configuration.Update(d => d.HasName("UpdateApp")));
            MapToStoredProcedures(configuration => configuration.Delete(d => d.HasName("DeleteApp")));
        }
    }
}
