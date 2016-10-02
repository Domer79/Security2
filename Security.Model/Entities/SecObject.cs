using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.SecObject")]
    public abstract class SecObject : ModelBase, ISecObject
    {
        [Key]
        public int IdSecObject { get; set; }

        [Required]
        [StringLength(200)]
        public string ObjectName { get; set; }

        [Column("type1")]
        public string Description { get; set; }

//        [StringLength(100)]
//        public string type1 { get; set; }
//
//        [StringLength(100)]
//        public string type2 { get; set; }
//
//        [StringLength(100)]
//        public string type3 { get; set; }
//
//        [StringLength(100)]
//        public string type4 { get; set; }
//
//        [StringLength(100)]
//        public string type5 { get; set; }
//
//        [StringLength(100)]
//        public string type6 { get; set; }
//
//        [StringLength(100)]
//        public string type7 { get; set; }

        public HashSet<Grant> Grants { get; set; }

        /// <summary>
        /// ���������� ������, ������� ������������ ������� ������.
        /// </summary>
        /// <returns>
        /// ������, �������������� ������� ������.
        /// </returns>
        public override sealed string ToString()
        {
            return ObjectName;
        }
    }
}
