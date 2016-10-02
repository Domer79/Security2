using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.RoleOfMember")]
    public class RoleOfMember : ModelBase
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdRole { get; set; }

        [StringLength(200)]
        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdMember { get; set; }

        [StringLength(200)]
        public string MemberName { get; set; }

        public bool IsUser { get; set; }

        public virtual Role Role { get; set; }
    }
}
