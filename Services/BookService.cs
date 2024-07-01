using AutoMapper;
using BooksApiApp.Data;
using BooksApiApp.DTO;
using BooksApiApp.Repositories;


namespace BooksApiApp.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<UserService>? _logger;
        private readonly IMapper? _mapper;

        public BookService(IUnitOfWork? unitOfWork, ILogger<UserService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookReadOnlyDTO>?> GetBooksAsync()
        {
            try
            {
                var books = await _unitOfWork!.BookRepository.GetAllAsync();

                // Map books to DTOs
                return books.Select(book => _mapper!.Map<BookReadOnlyDTO>(book));
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }
        public async Task<IEnumerable<BookReadOnlyDTO>?> GetBooksByTitleAsync(string title)
        {
            List<Book> books;
            try
            {
                books = await _unitOfWork!.BookRepository.GetBooksWithTitle(title);

                if (books != null)
                {
                    _logger!.LogInformation("{Message}", "Book with title: " + title + " was found");
                }
                else
                {
                    _logger!.LogWarning("{Message}", "Book: with title: " + title + "not found or invalid credentials provided");
                }
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return books?.Select(book => _mapper!.Map<BookReadOnlyDTO>(book)) ?? Enumerable.Empty<BookReadOnlyDTO>();
        }

        public async Task<BookReadOnlyDTO>? GetBookByIdAsync(int id)
        {
            Book? book;
            try
            {
                book = await _unitOfWork!.BookRepository.GetAsync(id);

                if (book != null)
                {
                    _logger!.LogInformation("{Message}", "Book with id: " + id + " was found");
                }
                else
                {
                    _logger!.LogWarning("{Message}", "Book: with id: " + id + "not found or invalid credentials provided");
                }
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            return _mapper!.Map<BookReadOnlyDTO>(book);
        }
    }
}

