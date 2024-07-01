using BooksApiApp.Data;

namespace BooksApiApp.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(string username, string password);
        Task<User?> UpdateUserAsync(int userId, User user);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByPhoneNumber(string phoneNumber);
       
       
    }
}
