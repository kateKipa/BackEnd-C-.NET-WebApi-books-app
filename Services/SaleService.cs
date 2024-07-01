using AutoMapper;
using BooksApiApp.Data;
using BooksApiApp.DTO;
using BooksApiApp.Model;
using BooksApiApp.Repositories;
using System.Linq;


namespace BooksApiApp.Services
{
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<UserService>? _logger;
        private readonly IMapper? _mapper;

        public SaleService(IUnitOfWork? unitOfWork, ILogger<UserService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }


        /// <summary>
        /// Retrieves a list of books the user is buying asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A list of books the user is buying.</returns>
        public async Task<List<BuyingSellingBooksByIdDTO>> GetBuyingBooksByUserAsync(int id)
        {
            try
            {
                var buyingList = await _unitOfWork!.SaleRepository.GetUsersBuyingBooksAsync(id);

                if (buyingList == null)
                {
                    _logger!.LogWarning("No buying records found for user with ID {UserId}", id);
                    return new List<BuyingSellingBooksByIdDTO>();       // Return empty list
                }

                var buyingBooks = _mapper!.Map<List<BuyingSellingBooksByIdDTO>>(buyingList);
                return buyingBooks;

            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);

                throw;
            }
        }


        /// <summary>
        /// Retrieves a list of books the user is selling asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A list of books the user is selling.</returns>
        public async Task<List<BuyingSellingBooksByIdDTO>> GetSellingBooksByUserAsync(int id)
        {
            try
            {
                var sellingList = await _unitOfWork!.SaleRepository.GetUsersSellingBooksAsync(id);

                if (sellingList == null)
                {
                    _logger!.LogWarning("No sales records found for user with ID {UserId}", id);
                    return new List<BuyingSellingBooksByIdDTO>();               // Return empty list
                }

                var sellingBooks = _mapper!.Map<List<BuyingSellingBooksByIdDTO>>(sellingList);
                return sellingBooks;
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);

                throw;
            }
        }
    }
}
