using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.UserGroupsDetail")]
    public class UserGroupsDetail : ModelBase
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdUser { get; set; }

        public string Login { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Usersid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdGroup { get; set; }

        public string GroupName { get; set; }

        public string GroupDescription { get; set; }
    }
}
