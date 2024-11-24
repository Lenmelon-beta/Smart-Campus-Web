using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomAuth.Models
{
    [Table("Chat", Schema = "dbo")]
    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column("From", TypeName = "nvarchar(max)")]
        public string From { get; set; }

        [Required]
        [Column("To", TypeName = "nvarchar(max)")]
        public string To { get; set; }

        [Required]
        [Column("Message", TypeName = "nvarchar(max)")]
        public string Message { get; set; }
    }
}