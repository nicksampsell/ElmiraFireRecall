using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace ElmiraFireRecall.Models
{
    public class MessageHistory : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int MessageTypeId { get; set; }
        public MessageType MessageType { get; set; }
        public List<FireGroup>? Groups { get; set; }
        public List<FireRecipient>? Recipients { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public string? Subject { get; set; }
        public string Message { get; set; }

        public string? LegacyUser { get; set; }
        [Column("Legacy")]
        public bool? IsLegacy { get; set; }
        public string? LegacyIPAddress { get; set; }


    }
}
