using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Security.Interfaces.Model;
using Security.Model.Base;
using Tools.Extensions;

namespace Security.Model.Entities
{
    [Table("sec.UsersView")]
    public class User : ModelBase, IUser
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

        IList<IGroup> IUser.Groups
        {
            get
            {
                return new List<IGroup>(Groups);
            }
        }
    }
}
