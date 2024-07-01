using AutoMapper;
using Azure.Core;
using BooksApiApp.Data;
using BooksApiApp.DTO;
using BooksApiApp.Model;
using BooksApiApp.Repositories;
using BooksApiApp.Security;
using BooksApiApp.Services.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace BooksApiApp.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<UserService>? _logger;
        private readonly IMapper? _mapper;

        public UserService(IUnitOfWork? unitOfWork, ILogger<UserService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a JWT token for the user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <param name="username">The user's username.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="userRole">The user's role.</param>
        /// <param name="appSecurityKey">The application security key.</param>
        /// <returns>A JWT token as a string.</returns>
        public string CreateUserToken(int userId, string? username, string? email, UserRole? userRole,
            string? appSecurityKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey!));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsInfo = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username!),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email!),
                new Claim(ClaimTypes.Role, userRole.ToString()!)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimsInfo),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JsonWebTokenHandler();
            var userToken = tokenHandler.CreateToken(tokenDescriptor);

            return userToken;           //"Bearer" + userToken
        }


        /// <summary>
        /// Deletes a user asynchronously by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="UserNotFoundException">Thrown if the user is not found.</exception>
        public async Task DeleteUserAsync(int id)
        {
            bool deleted;
            try
            {
                deleted = await _unitOfWork!.UserRepository.DeleteAsync(id);
                if (!deleted)
                {
                    throw new UserNotFoundException("User Not Found");
                }
                await _unitOfWork.SaveAsync(); 
                _logger!.LogInformation("User with ID {UserId} deleted successfully.", id);
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Gets a user by their username asynchronously.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            User? user;
            try
            {
                user = await _unitOfWork!.UserRepository.GetByUsernameAsync(username);
                _logger!.LogInformation("{Message}", "User: " + user + " found and returned");
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return user;
        }


        /// <summary>
        /// Signs up a new user asynchronously.
        /// </summary>
        /// <param name="request">The sign-up request containing user details.</param>
        /// <returns>The signed-up user if successful; otherwise, null.</returns>
        /// <exception cref="UserAlreadyExistsException">Thrown if a user with the same username already exists.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the service dependencies are not initialized.</exception>
        public async Task<User?> SignUpUserAsync(UserSignUpDTO request)
        {
            if (_unitOfWork == null || _mapper == null || _logger == null)
            {
                throw new InvalidOperationException("Service dependencies are not initialized.");
            }

            User user = _mapper!.Map<User>(request);

            try
            {
                User? existingUser = await _unitOfWork.UserRepository
                    .GetByUsernameAsync(user.Username);

                if (existingUser != null)
                {
                    throw new UserAlreadyExistsException($"User with username: {existingUser.Username} exists");
                }

                user.Password = EncryptionUtil.Encrypt(user.Password);

                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveAsync();

                _logger!.LogInformation("{Message}", "User: " + user + "signup success");

                return user;

            } catch (Exception ex)
            {
                _logger.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Updates a user asynchronously by ID.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userDTO">The user data transfer object containing updated information.</param>
        /// <returns>The updated user if successful; otherwise, null.</returns>
        public async Task<User?> UpdateUserAsync(int id, UserDTO userDTO)
        {
            User? existingUser;
            User? user;

            try
            {
                existingUser = await _unitOfWork!.UserRepository.GetAsync(id);
                if (existingUser == null) return null;

                _mapper!.Map(userDTO, existingUser);
                existingUser.Password = EncryptionUtil.Encrypt(existingUser.Password);

                var updatedUser = await _unitOfWork.UserRepository.UpdateUserAsync(id, existingUser);
                await _unitOfWork.SaveAsync();

                _logger!.LogInformation("{Message}", "User: " + updatedUser + " updated successfully");
                return updatedUser;
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            
        }


        /// <summary>
        /// Verifies the user's credentials and retrieves the user asynchronously.
        /// </summary>
        /// <param name="credentials">The user's login credentials.</param>
        /// <returns>The user if found and verified; otherwise, null.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the service dependencies are not initialized.</exception>
        
        public async Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials)
        {
            if (_unitOfWork == null || _logger == null)
            {
                throw new InvalidOperationException("Service dependencies are not initialized.");
            }

            User? user = null;

            try
            {
                user = await _unitOfWork!.UserRepository.GetUserAsync(credentials.Username!, credentials.Password!);
                if (user != null)
                {
                    _logger!.LogInformation("{{Message}}" + "User: {username} found and returned", user.Username);
                }
                else
                {
                    _logger.LogWarning("{Message}", "User: " + credentials.Username +  "not found or invalid credentials provided");
                }

                return user;

            } catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a user by their ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? user;
            try
            {
                user = await _unitOfWork!.UserRepository.GetAsync(id);

                if (user != null)
                {
                    _logger!.LogInformation("{Message}", "User with id: " + id + " Success");
                }
                else
                {
                    _logger!.LogWarning("{Message}", "User: with id: " + id + "not found or invalid credentials provided");
                }
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return user;
        }

        /// <summary>
        /// Retrieves a user by their phone number asynchronously.
        /// </summary>
        /// <param name="phoneNumber">The phone number of the user to retrieve.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        public async Task<User?> GetUserByPhoneNumber(string phoneNumber)
        {
            User? user;
            try
            {
                user = await _unitOfWork!.UserRepository.GetByPhoneNumber(phoneNumber);

                if (user != null)
                {
                    _logger!.LogInformation("{Message}", "User with phone number: " + phoneNumber + " was found");
                }
                else
                {
                    _logger!.LogWarning("{Message}", "User: with phone number: " + phoneNumber + " not found or invalid credentials provided");
                }
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return user;
        } 

    }
}
