using AutoMapper;
using BooksApiApp.Data;
using BooksApiApp.DTO;
using BooksApiApp.Model;
using BooksApiApp.Repositories;
using static System.Reflection.Metadata.BlobBuilder;

namespace BooksApiApp.Services
{
    public class BookForSaleService : IBookForSaleService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<UserService>? _logger;
        private readonly IMapper? _mapper;

        public BookForSaleService(IUnitOfWork? unitOfWork, ILogger<UserService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of books for sale by a specific user asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A list of books for sale.</returns>
        public async Task<List<BookForSale>> GetBooksForSaleByUserAsync(int id)
        {
            List<BookForSale> bookForSale;

            try
            {
                bookForSale = await _unitOfWork!.BookForSaleRepository.GetUsersBooksForSaleAsync(id);
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return bookForSale;
        }

        
        public async Task<List<BookForSale>> GetBooksForSaleByUserAsync(string username)
        {
            List<BookForSale> bookForSale;

            try
            {
                bookForSale = await _unitOfWork!.BookForSaleRepository.GetUsersBooksForSaleAsync(username);
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return bookForSale;
        }


        /// <summary>
        /// Adds a new book for sale asynchronously.
        /// </summary>
        /// <param name="bookForSaleDTO">The DTO containing the details of the book for sale.</param>
        /// <returns>The added book for sale.</returns>
        public async Task<BookForSale> AddBookForSaleAsync(BookForSaleDTO bookForSaleDTO)
        {
            try
            {
                // Map DTO to Book entity
                var book = _mapper!.Map<Book>(bookForSaleDTO);

                // Add the book and get the generated ID
                var addedBookId = await _unitOfWork!.BookRepository.AddAsyncWithReturnId(book);

                // Map DTO to BookForSale entity and set the BookId
                var bookForSale = _mapper.Map<BookForSale>(bookForSaleDTO);
                bookForSale.BookId = addedBookId;

                await _unitOfWork.BookForSaleRepository.AddAsync(bookForSale);
                await _unitOfWork.SaveAsync();


                return bookForSale;
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Retrieves all books for sale asynchronously.
        /// </summary>
        /// <returns>A list of books for sale.</returns>
        public async Task<IEnumerable<BookForSaleReadOnlyDTO>> GetBooksForSaleAsync()
        {
            try
            {

                var bookForSaleList = await _unitOfWork!.BookForSaleRepository.GetAllAsync();
                var bookForSaleReadOnlyDTOs = _mapper!.Map<IEnumerable<BookForSaleReadOnlyDTO>>(bookForSaleList);

                return bookForSaleReadOnlyDTOs;

            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }

    }
}
