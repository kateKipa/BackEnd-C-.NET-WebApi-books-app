using AutoMapper;
using BooksApiApp.Data;
using BooksApiApp.DTO;
using BooksApiApp.Repositories;
using BooksApiApp.Services.Exceptions;

namespace BooksApiApp.Services
{
    public class BookReadyForTradingService : IBookReadyForTradingService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<UserService>? _logger;
        private readonly IMapper? _mapper;

        public BookReadyForTradingService(IUnitOfWork? unitOfWork, ILogger<UserService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of books ready for trading by a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of books ready for trading.</returns>
        public async Task<List<BookReadyForTradingDTO>> GetBooksReadyForTradingAsync(int userId)
        {
            try
            {
                var booksReadyForTrading = await _unitOfWork!.BookReadyForTradingRepository.GetBooksReadyForTradingByUserAsync(userId);

                return _mapper!.Map<List<BookReadyForTradingDTO>>(booksReadyForTrading);
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Confirms the reception of a book, marking it as sold and removing it from the ready for trading list.
        /// </summary>
        /// <param name="bookId">The ID of the book.</param>
        /// <param name="userId">The ID of the user confirming the reception.</param>
        /// <returns>True if the operation was successful, otherwise false.</returns>
        public async Task<bool> ConfirmReceivedAsync(int bookId, int userId)
        {
            try
            {
                var bookReadyForTrading = await _unitOfWork!.BookReadyForTradingRepository.GetBookReadyForTradingByBookIdAndBuyerIdAsync(bookId, userId);
                if (bookReadyForTrading == null)
                {
                    throw new BookNotFoundException("Book not found in the ready for trading list");
                }

                var sale = new Sale
                {
                    BookId = bookReadyForTrading.BookId,
                    SellerId = bookReadyForTrading.SellerId,
                    BuyerId = bookReadyForTrading.BuyerId,
                    Price = bookReadyForTrading.Price,
                    PaymentMethod = bookReadyForTrading.PaymentMethod,
                    TypeOfTransaction = bookReadyForTrading.TypeOfTransaction,
                    ConditionOfBook = bookReadyForTrading.ConditionOfBook
                };

                await _unitOfWork.SaleRepository.AddAsync(sale);
                await _unitOfWork.BookReadyForTradingRepository.DeleteAsync(bookReadyForTrading.Id);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }

    }

}
