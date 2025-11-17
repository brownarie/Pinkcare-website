using System.ComponentModel.DataAnnotations;
namespace PinkCare.Models
{
    public class UserInfo
    {
        [Key]
        public int UserID { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [Required, StringLength(50)]
        public string Role { get; set; } = "User"; // e.g., Admin, User, Guest

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // navigation properties
        
    }
}
