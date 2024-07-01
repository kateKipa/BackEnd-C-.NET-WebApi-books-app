using AutoMapper;
using BooksApiApp.Data;
using BooksApiApp.DTO;
using BooksApiApp.Services;
using BooksApiApp.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BooksApiApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BooksForSaleController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksForSaleController> _logger;

        public BooksForSaleController(IApplicationService applicationService,IConfiguration configuration,IMapper mapper,ILogger<BooksForSaleController> logger)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Adds a book for sale.
        /// </summary>
        /// <param name="bookForSaleDTO">The book for sale DTO.</param>
        /// <returns>The added book for sale.</returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(BookForSale), 200)]
        [ProducesResponseType(400)]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<IActionResult> AddBookForSale([FromBody] BookForSaleDTO bookForSaleDTO)
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"];

            if (AppUser == null)
            {
                _logger.LogWarning("User is not logged in.");
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value!.Errors.Any())
                    .Select(e => new
                    {
                        Field = e.Key,
                        Errors = e.Value!.Errors.Select(error => error.ErrorMessage).ToArray()
                    });

                _logger.LogWarning("Model state is invalid: {Errors}", errors);
                return BadRequest(new { Errors = errors });
                
            }
            if (_applicationService == null)
            {
                _logger.LogError("Application service is null.");
                throw new ServerGenericException("Application service is null");
            }

            bookForSaleDTO.SellerId = AppUser.Id;


            var addedBookForSale = await _applicationService.BookForSaleService.AddBookForSaleAsync(bookForSaleDTO);
            _logger.LogInformation("Book for sale added successfully: {@AddedBookForSale}", addedBookForSale);
            return Ok(addedBookForSale);

        }

        //<summary>
        // Gets the list of books for sale.
        // </summary>
        // <returns>The list of books for sale.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookForSaleReadOnlyDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBooksForSale()
        {
            var booksForSaleReadOnlyDTO = await _applicationService.BookForSaleService.GetBooksForSaleAsync();
            return Ok(booksForSaleReadOnlyDTO);
        }

       

    }
}
