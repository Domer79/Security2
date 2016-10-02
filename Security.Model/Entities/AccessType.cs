using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.AccessType")]
    public class AccessType : ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAccessType { get; set; }

        [Required]
        [StringLength(300)]
        [Column("name")]
        [Index("IX_AccessType_Name", IsUnique = true)]
        public string Name { get; set; }

        public HashSet<Grant> Grants { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
