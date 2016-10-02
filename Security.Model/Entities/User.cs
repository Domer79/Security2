using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Security.Model.Base;
using Tools.Extensions;

namespace Security.Model.Entities
{
    [Table("sec.Users")]
    public class User : ModelBase
    {
        private string _usersid;

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать логин!"), StringLength(200)]
        public string Login { get; set; }

        [StringLength(200)]
        public string DisplayName { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

//        [Required]
        public byte[] Password { get; set; }
    }
}
