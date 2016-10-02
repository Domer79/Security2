using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.Settings")]
    internal class Settings : ModelBase
    {
        [Key]
        public int IdSettings { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}