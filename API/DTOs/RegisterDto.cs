using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
  public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,12}$", 
            ErrorMessage = "Password must be 8-12 characters long and contain at least one each of uppercase, lowercase, and numeric characters.")]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }
    }
}