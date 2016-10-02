using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.EntityConfigurations;

namespace Security.Model.Entities
{
    [Table("sec.UserGroups")]
    public class UserGroups
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdUser { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdGroup { get; set; }

    }

    public class UserGroupsConfiguration : BaseConfiguration<UserGroups>
    {
        public UserGroupsConfiguration()
        {
        }
    }
}
