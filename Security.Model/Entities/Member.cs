using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Interfaces.Model;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.Members")]
    public class Member : ModelBase, IMember
    {
        protected Member()
        {
            Roles = new HashSet<Role>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMember { get; set; }

        [Required]
        [Column("name")]
        [Index("IX_Member_Name", IsUnique = true)]
        public string Name { get; set; }

        public HashSet<Role> Roles { get; set; }

        IList<IRole> IMember.Roles
        {
            get
            {
                return new List<IRole>(Roles);
            }
        }
    }
}
