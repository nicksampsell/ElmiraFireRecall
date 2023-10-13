using System.ComponentModel.DataAnnotations;

namespace ElmiraFireRecall.Models
{
    public class MessageType : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
