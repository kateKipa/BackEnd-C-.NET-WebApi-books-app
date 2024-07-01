using BooksApiApp.DTO;

namespace BooksApiApp.Services
{
    public interface IApprovalSaleService
    {
        Task<bool> RequestPurchaseAsync(PurchaseRequestDTO purchaseRequestDTO);

        Task<List<BuyingSellingBooksByIdDTO>> GetApprovalSalesByUserAsync(int sellerId);

        Task<List<BuyingSellingBooksByIdDTO>> GetBooksForNeedApprovalAsync(int buyerId);

        Task<bool> ApproveSaleAsync(int bookId, int sellerId);

        Task<bool> RejectSaleAsync(int bookId, int sellerId);


    }
}
