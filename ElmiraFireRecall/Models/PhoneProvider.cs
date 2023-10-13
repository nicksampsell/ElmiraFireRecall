using System.ComponentModel.DataAnnotations;

namespace ElmiraFireRecall.Models
{
    public class PhoneProvider : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailSuffix { get; set; }
    }
}
