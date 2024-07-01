using BooksApiApp.Data;
using BooksApiApp.DTO;
using System.Reflection;

namespace BooksApiApp.Services
{
    public interface IBookForSaleService
    {
        Task<List<BookForSale>> GetBooksForSaleByUserAsync(int id);
        Task<List<BookForSale>> GetBooksForSaleByUserAsync(string username);
        Task<BookForSale> AddBookForSaleAsync(BookForSaleDTO bookForSaleDTO);
        Task<IEnumerable<BookForSaleReadOnlyDTO>> GetBooksForSaleAsync();
    }
}
