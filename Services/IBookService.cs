using BooksApiApp.Data;
using BooksApiApp.DTO;

namespace BooksApiApp.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookReadOnlyDTO>?> GetBooksAsync();
        Task<IEnumerable<BookReadOnlyDTO>?> GetBooksByTitleAsync(string title);

        Task<BookReadOnlyDTO>? GetBookByIdAsync(int id);

    }
}
