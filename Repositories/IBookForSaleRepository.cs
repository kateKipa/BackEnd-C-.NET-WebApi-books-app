using BooksApiApp.Data;

namespace BooksApiApp.Repositories
{
    public interface IBookForSaleRepository
    {
        Task<List<BookForSale>> GetUsersBooksForSaleAsync(int id);
        Task<List<BookForSale>> GetUsersBooksForSaleAsync(string username);

        Task<BookForSale?> GetBookForSaleByBookIdAndSellerIdAsync(int bookId, int sellerId);


    }
}
