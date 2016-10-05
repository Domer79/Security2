using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Security.Model.Base;

namespace Security.Model.Entities
{
    [Table("sec.SecObjects")]
    public class SecObject : ModelBase
    {
        [Key]
        public int IdSecObject { get; set; }

        [Required]
        [StringLength(200)]
        public string ObjectName { get; set; }

        public HashSet<Grant> Grants { get; set; }

        /// <summary>
        /// Возвращает строку, которая представляет текущий объект.
        /// </summary>
        /// <returns>
        /// Строка, представляющая текущий объект.
        /// </returns>
        public override sealed string ToString()
        {
            return ObjectName;
        }
    }
}
