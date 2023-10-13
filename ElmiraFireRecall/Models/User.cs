using System.ComponentModel.DataAnnotations;

namespace ElmiraFireRecall.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; } = false;
        public UserRole UserRole { get; set; } = UserRole.User;
    }

    public enum UserRole
    {
        User,
        Supervisor,
        Administrator,
        Developer
    }
}
