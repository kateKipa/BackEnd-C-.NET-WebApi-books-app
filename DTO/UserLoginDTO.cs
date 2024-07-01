using System.ComponentModel.DataAnnotations;

namespace BooksApiApp.DTO
{
    public class UserLoginDTO
    {

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username/Email should be between 2 and 50 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}
