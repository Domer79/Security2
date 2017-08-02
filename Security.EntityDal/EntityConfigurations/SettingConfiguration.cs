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
    /// <summary>
    /// Кофигурация сущности "Настройка"
    /// </summary>
    public class SettingConfiguration: BaseConfiguration<Setting>
    {
        public SettingConfiguration()
        {
            ToTable("Settings");
            HasKey(e => e.IdSettings);
            Property(e => e.IdSettings).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Name)
                .IsUnicode()
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("UQ_Settings_Name") {IsUnique = true}));

            Property(e => e.Description).IsUnicode();

            Property(e => e.Value).IsUnicode();
        }
    }
}
