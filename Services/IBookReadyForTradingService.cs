using BooksApiApp.Data;
using BooksApiApp.DTO;

namespace BooksApiApp.Services
{
    public interface IBookReadyForTradingService
    {
        Task<List<BookReadyForTradingDTO>> GetBooksReadyForTradingAsync(int userId);

        Task<bool> ConfirmReceivedAsync(int bookId, int userId);
    }
}
