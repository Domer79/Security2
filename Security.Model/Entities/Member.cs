using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.Members")]
    public class Member : ModelBase
    {
        public Member()
        {
            Roles = new HashSet<Role>();
        }

        [Key]
        public int IdMember { get; set; }

        [Required]
        [Column("name")]
        [Index("IX_Member_Name", IsUnique = true)]
        public string Name { get; set; }

        public bool IsUser { get; set; }

        public HashSet<Role> Roles { get; set; }
    }
}
