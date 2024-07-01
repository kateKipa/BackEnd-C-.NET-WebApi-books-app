using BooksApiApp.Repositories;
using BooksApiApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BooksApiApp.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(BooksWebApiContext context) : base(context)
        {
        }

        public async Task<List<Book>> GetBooksWithTitle(string title)
        {
            List<Book> books;

            books = await _context.Books
                        .Where(b => b.Title == title)
                        .ToListAsync();

            return books;

        }

      
    }
}
