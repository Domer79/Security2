using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.GroupsView")]
    public class Group : ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMember { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public HashSet<User> Users { get; set; }
    }
}
