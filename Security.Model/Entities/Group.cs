using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.Groups")]
    public class Group : ModelBase
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGroup { get; set; }

        [Required]
        [StringLength(200)]
        [Column("name")]
        public string GroupName { get; set; }

        public string Description { get; set; }
    }
}
