using System.ComponentModel.DataAnnotations;

namespace PinkCare.Models
{
    public class donation
      {
            [Key]
            public int DonationID { get; set; }

            [Required(ErrorMessage = "Donor name is required")]
            [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
            [Display(Name = "Full Name")]
            public string FullNames { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Resource type is required")]
            [StringLength(100, ErrorMessage = "Resource type cannot exceed 100 characters")]
            [Display(Name = "Resource Type")]
            public string ResourceType { get; set; } = string.Empty;

            [Required(ErrorMessage = "Quantity is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
            public int Quantity { get; set; }

            [Phone(ErrorMessage = "Invalid phone number")]
            [Display(Name = "Contact Number")]
            public string? ContactNumber { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.Now;
        }
}
