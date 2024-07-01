using BooksApiApp.Data;

namespace BooksApiApp.Repositories
{
    public interface IBookReadyForTradingRepository
    {
        Task<IEnumerable<BookReadyForTrading>> GetBooksReadyForTradingByUserAsync(int userId);

        Task<BookReadyForTrading?> GetBookReadyForTradingByBookIdAndBuyerIdAsync(int bookId, int buyerId);
    }
}
