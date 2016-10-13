using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Interfaces.Model;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.Grants")]
    public class Grant : ModelBase, IGrant
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdSecObject { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdRole { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdAccessType { get; set; }

        public Role Role { get; set; }
        public AccessType AccessType { get; set; }
        public SecObject SecObject { get; set; }

        IRole IGrant.Role
        {
            get { return Role; }
            set { Role = (Role)value; }
        }

        IAccessType IGrant.AccessType
        {
            get { return AccessType; }
            set { AccessType = (AccessType)value; }
        }

        ISecObject IGrant.SecObject
        {
            get { return SecObject; }
            set { SecObject = (SecObject)value; }
        }
    }
}
