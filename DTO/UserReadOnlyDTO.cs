using BooksApiApp.Model;

namespace BooksApiApp.DTO
{
    public class UserReadOnlyDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Password { get; set; }
        public UserRole UserRole { get; set; } = UserRole.SellerBuyer;
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? Number { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
