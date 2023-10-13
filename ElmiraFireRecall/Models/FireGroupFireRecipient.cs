using Microsoft.EntityFrameworkCore;

namespace ElmiraFireRecall.Models
{
    [Keyless]
    public class FireGroupFireRecipient
    {
        public int GroupsId { get; set; }
        public int RecipientsId { get; set; }
    }
}
