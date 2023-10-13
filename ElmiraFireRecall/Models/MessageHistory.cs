using System.ComponentModel.DataAnnotations;
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

        public string? Subject { get; set; }
        public string Message { get; set; }


    }
}
