using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.Groups")]
    public class Group : Member
    {
        public string Description { get; set; }

        public HashSet<User> Users { get; set; }
    }
}
