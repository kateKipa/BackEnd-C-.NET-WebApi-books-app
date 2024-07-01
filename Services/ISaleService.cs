using BooksApiApp.DTO;

namespace BooksApiApp.Services
{
    public interface ISaleService
    {
        Task<List<BuyingSellingBooksByIdDTO>> GetBuyingBooksByUserAsync(int id);
        Task<List<BuyingSellingBooksByIdDTO>> GetSellingBooksByUserAsync(int id);
    }
}
