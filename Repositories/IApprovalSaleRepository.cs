using BooksApiApp.Data;

namespace BooksApiApp.Repositories
{
    public interface IApprovalSaleRepository
    {
        Task<List<ApprovalSale>> GetApprovalSalesBySellerIdAsync(int sellerId);
        Task<List<ApprovalSale>> GetBooksThatUserAsksForApproveAsync(int buyerId);

        Task<ApprovalSale?> GetApprovalSaleByBookForSaleIdAsync(int bookForSaleId);
    }
}
