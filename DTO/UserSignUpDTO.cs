using BooksApiApp.Model;
using System.ComponentModel.DataAnnotations;

namespace BooksApiApp.DTO
{
    public class UserSignUpDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username should be between 2 and 50 characters.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, ErrorMessage = "Email should not exceed 50 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100)]
        [RegularExpression(@"^(?=.*\d).{6,}$",
        ErrorMessage = "Password must contain at least one digit and has a minimum length of 6 characters")]
        public string? Password { get; set; } = null!;

        [Required(ErrorMessage = "Firstname is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Firstname should be between 2 and 50 characters.")]
        public string Firstname { get; set; } = null!;

        [Required(ErrorMessage = "Lastname is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Lastname should be between 2 and 50 characters.")]
        public string Lastname { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?[0-9]{1,12}$", ErrorMessage = "Phone number must be a numeric value" +
            " between 1 and 12 digits, optionally starting with a +.")]
        public string PhoneNumber { get; set; } = null!;

        public UserRole UserRole { get; set; } = UserRole.SellerBuyer;

        [StringLength(50, ErrorMessage = "City should not exceed 50 characters.")]
        public string? City { get; set; }

        [StringLength(50, ErrorMessage = "Address should not exceed 50 characters.")]
        public string? Address { get; set; }

        [StringLength(10, ErrorMessage = "Number should not exceed 10 characters.")]
        public string? Number { get; set; }

        public bool IsDeleted = false;
    }
}
