using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElmiraFireRecall.Models
{
    public class FireRecipient : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneProviderId { get; set; }
        public PhoneProvider? PhoneProvider { get; set; }
        public List<FireGroup>? FireGroups { get; set; } = new List<FireGroup>();
        [NotMapped]
        public List<FireGroupFireRecipient> GroupLink { get; set; } = new List<FireGroupFireRecipient>();
        

    }
}
