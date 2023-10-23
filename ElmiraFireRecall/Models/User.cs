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
        public List<MessageHistory>? MessageHistory { get; set; }

        public string FullName { get => $"{FirstName} {LastName}"; }
    }

    public enum UserRole
    {
        User = 0,
        Administrator = 2
    }
}
