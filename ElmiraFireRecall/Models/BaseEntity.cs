namespace ElmiraFireRecall.Models
{
    public class BaseEntity
    {
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
    }
}
