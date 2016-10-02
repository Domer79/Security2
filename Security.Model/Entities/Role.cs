using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec._Role")]
    public class Role : ModelBase
    {
        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="T:System.Object"/>.
        /// </summary>
        public Role()
        {
            Grants = new HashSet<Grant>();
        }

        [Key]
        public int IdRole { get; set; }

        [Required]
        [StringLength(200)]
        [Column("name")]
        [Index("IX_Role_Name", IsUnique = true)]
        public string Name { get; set; }

        public string Description { get; set; }

        public HashSet<Grant> Grants { get; set; }

        public HashSet<RoleOfMember> RoleOfMembers { get; set; }
    }
}
