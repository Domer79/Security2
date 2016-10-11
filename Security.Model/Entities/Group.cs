using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Interfaces.Model;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.GroupsView")]
    public class Group : ModelBase, IGroup
    {
        public Group()
        {
            Users = new HashSet<User>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMember { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public HashSet<User> Users { get; set; }

        IList<IUser> IGroup.Users
        {
            get
            {
                return new List<IUser>(Users);
            }
        }
    }
}
