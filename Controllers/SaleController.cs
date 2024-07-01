using AutoMapper;
using BooksApiApp.Data;
using BooksApiApp.Model;
using BooksApiApp.Services;
using BooksApiApp.Services.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace BooksApiApp.Controllers
{
    public class SaleController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleController"/> class.
        /// </summary>
        public SaleController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper, ILogger<UserController> logger)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the books being bought by the logged-in user.
        /// </summary>
        /// <returns>A list of books being bought by the user.</returns>
        [HttpGet]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<IActionResult> GetBuyingBooksByUser()
        {
            // Access the Authorization header
            var authorizationHeader = HttpContext.Request.Headers["Authorization"];

            var userId = this.AppUser?.Id;

            if (userId == null)
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            var buyingBooks = await _applicationService.SaleService.GetBuyingBooksByUserAsync(userId.Value);

            if (buyingBooks is null)
            {
                throw new TheListIsNullException("This list of books is null.");
            }
            return Ok(buyingBooks);
        }

        /// <summary>
        /// Retrieves the books being sold by the logged-in user.
        /// </summary>
        /// <returns>A list of books being sold by the user.</returns>
        [HttpGet]
        [Authorize(Roles = "SellerBuyer")]
        public async Task<IActionResult> GetSellingBooksByUser()
        {
            var userId = this.AppUser?.Id;

            if (userId == null)
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            var sellingBooks = await _applicationService.SaleService.GetSellingBooksByUserAsync(userId.Value);

            if (sellingBooks is null)
            {
                throw new TheListIsNullException("This list of books is null.");
            }
            return Ok(sellingBooks);
        }
    }
}

