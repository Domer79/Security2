using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.Users")]
    public class User : ModelBase, IUser
    {
        private string _usersid;

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "���������� ������� �����!"), StringLength(200)]
        public string Login { get; set; }

        [StringLength(200)]
        public string DisplayName { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Usersid
        {
            get { return _usersid; }
            set
            {
                if (value != null)
                    if (!value.RxIsMatch(@"^S-1-5-21-[\d]+-[\d]+-[\d]+-[\d]+[\d]$") && !value.RxIsMatch("^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$", RegexOptions.IgnoreCase))
                        throw new InvalidSidException();

                _usersid = value ?? Guid.NewGuid().ToString();
            }
        }

//        [Required]
        public byte[] Password { get; set; }

        #region IMember members

        [NotMapped]
        int IMember.IdMember
        {
            get { return IdUser; }
            set { IdUser = value; }
        }

        [NotMapped]
        string IMember.Name {
            get { return Login; }
            set { Login = value; }
        }

        #endregion
    }
}
