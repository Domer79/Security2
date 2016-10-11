using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Interfaces.Model;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.AccessTypes")]
    public class AccessType : ModelBase, IAccessType
    {
        public AccessType()
        {
            Grants = new HashSet<Grant>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAccessType { get; set; }

        [Required]
        [StringLength(300)]
        [Column("name")]
        [Index("IX_AccessType_Name", IsUnique = true)]
        public string Name { get; set; }

        public HashSet<Grant> Grants { get; set; }

        IList<IGrant> IAccessType.Grants
        {
            get { return new List<IGrant>(Grants); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
