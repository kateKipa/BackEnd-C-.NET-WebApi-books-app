using BooksApiApp.Data;

namespace BooksApiApp.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooksWithTitle(string title);


    }
}
