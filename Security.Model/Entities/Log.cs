using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Security.Model.Entities
{
    [Table("sec.Logs")]
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLog { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
