using System.ComponentModel.DataAnnotations;

namespace BooksApiApp.DTO
{
    public class UserPatchDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username should be between 2 and 50 characters.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Email should not exceed 50 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*\d).{6,}$",
        ErrorMessage = "Password must contain at least one digit")]
        public string? Password { get; set; } = null!;

    }
}
