using AutoMapper;
using BooksApiApp.Data;
using BooksApiApp.DTO;
using BooksApiApp.Services;
using BooksApiApp.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BooksApiApp.Controllers
{
    public class UserController : BaseController
    {

        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper, ILogger<UserController> logger)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Signs up a new user.
        /// </summary>
        /// <param name="userSignUpDTO">User sign-up details</param>
        /// <returns>Returns the created user</returns>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the input data is invalid</response>
        /// <response code="409">If a user with the provided email, username, or phone number already exists</response>
        /// <response code="500">If an unexpected error occurs</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserReadOnlyDTO>> SignupUserAsync(UserSignUpDTO? userSignUpDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value!.Errors.Any())
                    .Select(e => new
                    {
                        Field = e.Key,
                        Errors = e.Value!.Errors.Select(error => error.ErrorMessage).ToArray()
                    });

                // instead of return BadRequest(new { Errors = errors });
                throw new InvalidRegistrationException("ErrorsInRegistation: " + errors);
            }
            if (_applicationService == null)
            {
                throw new ServerGenericException("Application service is null");
            }

            try
            {
                // Check if the username is unique
                User? user = await _applicationService.UserService.GetUserByUsernameAsync(userSignUpDTO!.Username!);
                if (user is not null)
                {
                    throw new UserAlreadyExistsException("Username is already taken");
                }

                //// Check if the email is unique
                //user = await _applicationService.UserService.GetUserByEmailAsync(userSignUpDTO.Email);
                //if (user != null)
                //{
                //    return Conflict(new { msg = "Email already exists", errors = new { email = new[] { "Email is already registered" } } });
                //}


                // Check if the phone number is unique
                user = await _applicationService.UserService.GetUserByPhoneNumber(userSignUpDTO.PhoneNumber);
                if (user is not null)
                {
                    throw new PhonenumberAlreadyExistsException("There is already a user with this phone number");
                }

                User? returnedUser = await _applicationService.UserService.SignUpUserAsync(userSignUpDTO);
                if (returnedUser is null)
                {
                    throw new InvalidRegistrationException("User registration failed");
                }

                var returnedUserDTO = _mapper.Map<UserReadOnlyDTO>(returnedUser);

                return CreatedAtAction(nameof(GetUserById), new { id = returnedUserDTO.Id }, returnedUserDTO);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && sqlException.Number == 2601)
            {
                throw new UserAlreadyExistsException("A user with this email, username, or phone number already exists.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An unexpected error occurred.");
                throw;
                
            }
        }


        /// <summary>
        /// Retrieves the user by their ID.
        /// </summary>
        /// <returns>Returns the user details</returns>
        [HttpGet()]
        [Authorize(Roles = "SellerBuyer")]
        [ProducesResponseType(typeof(IEnumerable<UserReadOnlyDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserReadOnlyDTO>> GetUserById()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"];

            if (AppUser == null)
            {
                _logger.LogWarning("User is not logged in.");
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            var userId = this.AppUser.Id;

            var user = await _applicationService.UserService.GetUserByIdAsync(userId);

            if (user is null)
            {
                throw new UserNotFoundException("UserNotFound");
            }

            var returnedUser = _mapper.Map<UserReadOnlyDTO>(user);
            return Ok(returnedUser);
        }


        /// <summary>
        /// Retrieves the user by their username.
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>Returns the user details</returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<UserReadOnlyDTO>> GetUserByUsernameAsync(string? username)
        {
            var returnedUserDTO = await _applicationService.UserService.GetUserByUsernameAsync(username!);
            if (returnedUserDTO is null)
            {
                throw new UserNotFoundException("UserNotFound");
            }

            return Ok(returnedUserDTO);
        }


        /// <summary>
        /// Logs in the user.
        /// </summary>
        /// <param name="credentials">User login credentials</param>
        /// <returns>Returns the JWT token</returns>
        /// <response code="200">Returns the JWT token</response>
        /// <response code="401">If the username or password is incorrect</response>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<JwtTokenDTO>> LoginUserAsync(UserLoginDTO credentials)
        {

            var user = await _applicationService.UserService.VerifyAndGetUserAsync(credentials);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid Username or Password.");
            }

            var userToken = _applicationService.UserService.CreateUserToken(user.Id, user.Username!, user.Email!,
                (Model.UserRole?)user.UserRole, _configuration["Authentication:SecretKey"]!);

            JwtTokenDTO token = new()
            {
                Token = userToken
            };

            return Ok(token);
        }


        /// <summary>
        /// Updates the user's account details.
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <param name="userDTO">The user details to update</param>
        /// <returns>Returns the updated user details</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<ActionResult<UserDTO>> UpdateUserAccount(int id, [FromBody] UserDTO? userDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(e => e.Value!.Errors.Any())
                        .Select(e => new
                        {
                            Field = e.Key,
                            Errors = e.Value!.Errors.Select(error => error.ErrorMessage).ToArray()
                        });

                    // instead of return BadRequest(new { Errors = errors });
                    throw new InvalidRegistrationException("ErrorsInRegistation: " + errors);
                }

                if (_applicationService == null)
                {
                    throw new ServerGenericException("Application service is null");
                }

                var userId = AppUser!.Id;
                if (id != userId)
                {
                    throw new ForbiddenException("ForbiddenAccess");
                }

                // Check if the username is unique
                User? existingUser = await _applicationService.UserService.GetUserByUsernameAsync(userDTO!.Username);
                if (existingUser != null && existingUser.Id != userId)
                {
                    throw new UserAlreadyExistsException("Username is already taken");
                }

                //// Check if the email is unique
                //user = await _applicationService.UserService.GetUserByEmailAsync(userSignUpDTO.Email);
                //if (user != null)
                //{
                //    return Conflict(new { msg = "Email already exists", errors = new { email = new[] { "Email is already registered" } } });
                //}


                // Check if the phone number is unique
                existingUser = await _applicationService.UserService.GetUserByPhoneNumber(userDTO.PhoneNumber);
                if (existingUser != null && existingUser.Id != userId)
                {
                    throw new PhonenumberAlreadyExistsException("There is already a user with this phone number");
                }


                var userToUpdate = await _applicationService.UserService.UpdateUserAsync(userId, userDTO!);
                var returnedUserDTO = _mapper.Map<UserDTO>(userToUpdate);
                return Ok(returnedUserDTO);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && sqlException.Number == 2601)
            {
                throw new UserAlreadyExistsException("A user with this email, username, or phone number already exists.");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An unexpected error occurred.");
                throw;

            }
        }


        /// <summary>
        /// Deletes the user account.
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns>Returns a no content status</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userId = AppUser!.Id;
            if (id != userId)
            {
                throw new ForbiddenException("ForbiddenAccess");
            }

            await _applicationService.UserService.DeleteUserAsync(userId);
            return NoContent();
        }
    }
}

