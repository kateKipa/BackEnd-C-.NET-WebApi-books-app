using BooksApiApp.Repositories;
using BooksApiApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BooksApiApp.Repositories
{
    public class BookForSaleRepository : BaseRepository<BookForSale>, IBookForSaleRepository
    {
        public BookForSaleRepository(BooksWebApiContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a list of books for sale which are available by a specific user ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A list of available books for sale by the user.</returns>
        public async Task<List<BookForSale>> GetUsersBooksForSaleAsync(int id)
        {
            List<BookForSale> booksForSale;

            booksForSale = await _context.BookForSales
                                    .Where(bfs => bfs.SellerId == id && bfs.IsAvailable == true)
                                    .ToListAsync();
            return booksForSale;
        }


        /// <summary>
        /// Retrieves a list of books for sale which are available by a specific username.
        /// </summary>
        /// <param name="username">The username of the seller.</param>
        /// <returns>A list of available books for sale by the user.</returns>
        public async Task<List<BookForSale>> GetUsersBooksForSaleAsync(string username)
        {
            List<BookForSale> booksForSale;

            booksForSale = await _context.BookForSales
                                    .Include(bfs => bfs.Seller)
                                    .Where(bfs => bfs.Seller.Username == username && bfs.IsAvailable == true)
                                    .ToListAsync();
            return booksForSale;
        }

        /// <summary>
        /// Retrieves all books for sale which are available.
        /// </summary>
        /// <returns>A list of all available books for sale.</returns>
        public override async Task<IEnumerable<BookForSale>> GetAllAsync()
        {
            var entities = await _context.BookForSales
                .Include(bfs => bfs.Book)
                .Include(bfs => bfs.Seller)
                .Where(bfs => bfs.IsAvailable == true)
                .ToListAsync();
            return entities;
        }

        /// <summary>
        /// Retrieves a book for sale which are available, by book ID and seller ID.
        /// </summary>
        /// <param name="bookId">The ID of the book.</param>
        /// <param name="sellerId">The ID of the seller.</param>
        /// <returns>The book for sale if found; otherwise, null.</returns>
        public async Task<BookForSale?> GetBookForSaleByBookIdAndSellerIdAsync(int bookId, int sellerId)
        {
            return await _context.BookForSales
                 .Include(bfs => bfs.Book) 
                 .FirstOrDefaultAsync(b => b.BookId == bookId && b.SellerId == sellerId && !b.IsAvailable);
        }
    }
}
