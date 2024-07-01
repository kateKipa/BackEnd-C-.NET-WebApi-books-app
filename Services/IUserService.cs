using BooksApiApp.Data;
using BooksApiApp.DTO;
using BooksApiApp.Model;

namespace BooksApiApp.Services
{
    public interface IUserService
    {
        Task<User?> SignUpUserAsync(UserSignUpDTO request);
        Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials);
        Task<User?> UpdateUserAsync(int userId, UserDTO userDTO);
        Task<User?> GetUserByUsernameAsync(string username);
        string CreateUserToken(int userId, string? username, string? email, UserRole? userRole, string? appSecurityKey);
        //Task<UserReadOnlyDTO> GetUserByUsername(string username);
        Task DeleteUserAsync(int id);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByPhoneNumber(string phoneNumber);
       
       

    }
}
