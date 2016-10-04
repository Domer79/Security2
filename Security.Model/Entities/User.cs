using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Security.Model.Base;
using Tools.Extensions;

namespace Security.Model.Entities
{
    [Table("sec.Users")]
    public class User : Member
    {
        public User()
        {
            Groups = new HashSet<Group>();
        }

        [NotMapped]
        public string Login
        {
            get { return Name; }
            set { Name = value; }
        }
//        [Required]
        public byte[] Password { get; set; }

        public HashSet<Group> Groups { get; set; }
    }
}
