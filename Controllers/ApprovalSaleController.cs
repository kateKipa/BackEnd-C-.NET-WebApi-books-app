using AutoMapper;
using BooksApiApp.Data;
using BooksApiApp.DTO;
using BooksApiApp.Services;
using BooksApiApp.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksApiApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApprovalSaleController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<ApprovalSaleController> _logger;

        public ApprovalSaleController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper, ILogger<ApprovalSaleController> logger)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Allows a user to request the purchase of a book.
        /// </summary>
        /// <param name="request">The purchase request details.</param>
        /// <returns>A response indicating the success or failure of the purchase request.</returns>
        [HttpPost("RequestPurchase")]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<IActionResult> RequestPurchase([FromBody] PurchaseRequestDTO request)
        {
            try
            {
                if (_applicationService == null)
                {
                    throw new ServerGenericException("Application service is null");
                }

                var userId = AppUser!.Id;
                if (request.BuyerId != userId)
                {
                    throw new ForbiddenException("ForbiddenAccess");
                }


                var result = await _applicationService.ApprovalSaleService.RequestPurchaseAsync(request);
                if (!result)
                {
                    return BadRequest(new { message = "Purchase request failed" });
                }

                return Ok(new { message = "Purchase request successful" });

            }
            catch (BookNotFoundException ex)
            {
                // Return a 404 Not Found status with the custom error message
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all approval sales associated with the logged-in user.
        /// </summary>
        /// <returns>A list of approval sales.</returns>
        [HttpGet("GetApprovalSalesByUser")]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<IActionResult> GetApprovalSalesByUser()
        {
            try
            {
                if (_applicationService == null)
                {
                    throw new ServerGenericException("Application service is null");
                }

                if (AppUser == null)
                {
                    _logger.LogWarning("User is not logged in.");
                    throw new UnauthorizedAccessException("User is not logged in.");
                }

                var userId = AppUser!.Id;                   // AppUser is the logged-in user

                var approvalBooks = await _applicationService.ApprovalSaleService.GetApprovalSalesByUserAsync(userId);

                if (approvalBooks == null || !approvalBooks.Any())
                {
                    return NotFound("No books that need approval by this user were found.");
                }

                return Ok(approvalBooks);
            }
            catch (Exception ex)
            {
                _logger!.LogError(ex, "An error occurred while retrieving books sold by user.");

                throw;
            }
        }

        /// <summary>
        /// Retrieves all books that need approval from the logged-in user.
        /// </summary>
        /// <returns>A list of books that need approval.</returns>
        [HttpGet("GetBooksThatNeedApproval")]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<IActionResult> GetBooksThatNeedApproval()
        {
            try
            {
                if (_applicationService == null)
                {
                    throw new ServerGenericException("Application service is null");
                }

                if (AppUser == null)
                {
                    _logger.LogWarning("User is not logged in.");
                    throw new UnauthorizedAccessException("User is not logged in.");
                }

                var userId = AppUser!.Id;                   // AppUser is the logged-in user

                var approvalBooks = await _applicationService.ApprovalSaleService.GetBooksForNeedApprovalAsync(userId);

                if (approvalBooks == null || !approvalBooks.Any())
                {
                    return NotFound("No books that this user has ask for approval were found.");
                }

                return Ok(approvalBooks);
            }
            catch (Exception ex)
            {
                _logger!.LogError(ex, "An error occurred while retrieving books sold by user.");

                throw;
            }
        }

        /// <summary>
        /// Allows a user to approve the sale of a book.
        /// </summary>
        /// <param name="bookId">The ID of the book to approve.</param>
        /// <returns>A response indicating the success or failure of the approval.</returns>
        [HttpPost("ApproveSale")]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<IActionResult> ApproveSale([FromBody] int bookId)
        {
            try
            {
                if (_applicationService == null)
                {
                    throw new ServerGenericException("Application service is null");
                }

                var sellerId = AppUser!.Id;

                var result = await _applicationService.ApprovalSaleService.ApproveSaleAsync(bookId, sellerId);
                if (!result)
                {
                    return BadRequest(new { message = ("Approval failed") });
                }

                return Ok(new { message = ("Approval successful") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while approving the sale.");
                return StatusCode(500, "An error occurred while processing the request");
                throw;
            }
        }

        /// <summary>
        /// Allows a user to reject the sale of a book.
        /// </summary>
        /// <param name="bookId">The ID of the book to reject.</param>
        /// <returns>A response indicating the success or failure of the rejection.</returns>
        [HttpPost("RejectSale")]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<IActionResult> RejectSale([FromBody] int bookId)
        {
            try
            {
                if (_applicationService == null)
                {
                    throw new ServerGenericException("Application service is null");
                }

                var sellerId = AppUser!.Id;

                var result = await _applicationService.ApprovalSaleService.RejectSaleAsync(bookId, sellerId);
                if (!result)
                {
                    return BadRequest(new { message = ("Rejection failed") });
                }

                return Ok(new { message = ("Rejection successful") });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while rejecting the sale.");
                return StatusCode(500, "An error occurred while processing the request");
                throw;
            }
        }
    }
}
