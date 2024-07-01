using AutoMapper;
using BooksApiApp.Services;
using BooksApiApp.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksApiApp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BookReadyForTradingController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<BookReadyForTradingController> _logger;

        public BookReadyForTradingController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper, ILogger<BookReadyForTradingController> logger)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }


        /// <summary>
        /// Retrieves the books ready for trading for the logged-in user.
        /// </summary>
        /// <returns>A list of books ready for trading.</returns>
        [HttpGet("GetBooksReadyForTrading")]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<IActionResult> GetBooksReadyForTrading()
        {
            try
            {
                if (_applicationService == null)
                {
                    throw new ServerGenericException("Application service is null");
                }

                var userId = AppUser!.Id; 

                var booksReadyForTrading = await _applicationService.BookReadyForTradingService.GetBooksReadyForTradingAsync(userId);

                if (booksReadyForTrading == null || !booksReadyForTrading.Any())
                {
                    return NotFound(new { message = ("No books ready for trading found for this user.") });
                }

                return Ok(booksReadyForTrading);
            }
            catch (Exception ex)
            {
                _logger!.LogError(ex, "An error occurred while retrieving books ready for trading.");
                throw;
            }
        }


        /// <summary>
        /// Confirms the reception of a book by the buyer.
        /// </summary>
        /// <param name="bookId">The ID of the book.</param>
        /// <returns>Confirmation message.</returns>
        [HttpPost("ConfirmReceived")]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<IActionResult> ConfirmReceived([FromBody] int bookId)
        {
            try
            {
                if (_applicationService == null)
                {
                    throw new ServerGenericException("Application service is null");
                }

                var userId = AppUser!.Id;

                var result = await _applicationService.BookReadyForTradingService.ConfirmReceivedAsync(bookId, userId);
                if (!result)
                {
                    return BadRequest(new { message = "Confirmation failed" });
                }

                return Ok(new { message = "Book received successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while confirming the reception of the book.");
                return StatusCode(500, "An error occurred while processing the request");
                throw;
            }
        }


    }
}

