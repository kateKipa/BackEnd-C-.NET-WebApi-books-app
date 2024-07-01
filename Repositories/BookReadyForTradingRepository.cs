using BooksApiApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BooksApiApp.Repositories
{
    public class BookReadyForTradingRepository : BaseRepository<BookReadyForTrading>, IBookReadyForTradingRepository
    {
        public BookReadyForTradingRepository(BooksWebApiContext context) : base(context)
        {
        }


        /// <summary>
        /// Retrieves a list of books ready for trading by the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of books ready for trading that belong to the user.</returns>
        public async Task<IEnumerable<BookReadyForTrading>> GetBooksReadyForTradingByUserAsync(int userId)
        {
            var bookReadyForTrading = await _context.BookReadyForTradings
                                .Include(b => b.Buyer)
                                .Include(b => b.Seller)
                                .Where(b => b.BuyerId == userId).ToListAsync();
            return bookReadyForTrading;
        }


        /// <summary>
        /// Retrieves a book ready for trading by book ID and buyer ID.
        /// </summary>
        /// <param name="bookId">The ID of the book.</param>
        /// <param name="buyerId">The ID of the buyer.</param>
        /// <returns>The book ready for trading if found; otherwise, null.</returns>
        public async Task<BookReadyForTrading?> GetBookReadyForTradingByBookIdAndBuyerIdAsync(int bookId, int buyerId)
        {
            return await _context.BookReadyForTradings
                .FirstOrDefaultAsync(brft => brft.BookId == bookId && brft.BuyerId == buyerId);
        }

    }
}
