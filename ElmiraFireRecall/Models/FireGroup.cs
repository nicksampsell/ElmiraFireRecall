using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElmiraFireRecall.Models
{
    public class FireGroup : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string? Description { get; set; } = String.Empty;
        public List<FireRecipient> Recipients { get; set; } = new List<FireRecipient>();
        [NotMapped]
        public List<FireGroupFireRecipient> RecipientsLink = new List<FireGroupFireRecipient>();
        
    }
}
