using BooksApiApp.Data;

namespace BooksApiApp.Repositories
{
    public interface ISaleRepository
    {
        Task<List<Sale>> GetUsersBuyingBooksAsync(int id);
        Task<List<Sale>> GetUsersSellingBooksAsync(int id);
    }
}
