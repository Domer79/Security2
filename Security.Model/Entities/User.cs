using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Security.Model.Base;
using Tools.Extensions;

namespace Security.Model.Entities
{
    [Table("sec.UsersView")]
    public class User : ModelBase
    {
        public User()
        {
            Groups = new HashSet<Group>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMember { get; set; }

        public string Login { get; set; }

        public byte[] Password { get; set; }

        public HashSet<Group> Groups { get; set; }
    }
}
