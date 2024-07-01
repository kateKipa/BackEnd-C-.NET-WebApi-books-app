using BooksApiApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BooksApiApp.Repositories
{
    public class ApprovalSaleRepository : BaseRepository<ApprovalSale>, IApprovalSaleRepository
    {
        public ApprovalSaleRepository(BooksWebApiContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a list of approval sales by a specific seller ID.
        /// </summary>
        /// <param name="sellerId">The ID of the seller.</param>
        /// <returns>A list of approval sales by the seller.</returns>
        public async Task<List<ApprovalSale>> GetApprovalSalesBySellerIdAsync(int sellerId)
        {
            return await _context.ApprovalSales
                .Include(a => a.BookForSale)
                .ThenInclude(b => b.Book)
                .Include(a => a.Buyer)
                .Include(a => a.BookForSale.Seller)
                .Where(a => a.BookForSale.SellerId == sellerId)
                .ToListAsync();
        }


        /// <summary>
        /// Retrieves a list of books that a specific user has asked for approval.
        /// </summary>
        /// <param name="buyerId">The ID of the buyer.</param>
        /// <returns>A list of approval sales where the user is the buyer.</returns>
        public async Task<List<ApprovalSale>> GetBooksThatUserAsksForApproveAsync(int buyerId)
        {
            return await _context.ApprovalSales
                .Include(a => a.BookForSale)
                .ThenInclude(b => b.Book)
                .Include(a => a.Buyer)
                .Include(a => a.BookForSale.Seller)
                .Where(a => a.BuyerId == buyerId)
                .ToListAsync();
        }


        /// <summary>
        /// Retrieves an approval sale by the BookForSale ID.
        /// </summary>
        /// <param name="bookForSaleId">The ID of the book for sale.</param>
        /// <returns>The approval sale if found; otherwise, null.</returns>
        public async Task<ApprovalSale?> GetApprovalSaleByBookForSaleIdAsync(int bookForSaleId)
        {
            return await _context.ApprovalSales
               .Include(asb => asb.BookForSale) 
               .FirstOrDefaultAsync(asb => asb.BookForSaleId == bookForSaleId);
        }
    }

}
