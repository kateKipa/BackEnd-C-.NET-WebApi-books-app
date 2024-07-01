using BooksApiApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BooksApiApp.Repositories
{
    public class SaleRepository : BaseRepository<Sale>, ISaleRepository
    {
        public SaleRepository(BooksWebApiContext context) : base(context)
        {
        }


        /// <summary>
        /// Retrieves a list of books that the user has bought.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A list of sales representing the books the user has bought.</returns>
        public async Task<List<Sale>> GetUsersBuyingBooksAsync(int id)
        {
            List<Sale> userBuyingBooks;

            userBuyingBooks = await _context.Sales
                                    .Include(s => s.Book)
                                    .Include(s => s.Seller)
                                    .Where(s => s.BuyerId == id)
                                    .ToListAsync();

            return userBuyingBooks;
        }


        /// <summary>
        /// Retrieves a list of books that the user has sold.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A list of sales representing the books the user has sold.</returns>
        public async Task<List<Sale>> GetUsersSellingBooksAsync(int id)
        {
            List<Sale> userSellingBooks;

            userSellingBooks = await _context.Sales
                                    .Include(s => s.Book)
                                    .Include(s => s.Buyer) 
                                    .Where(s => s.SellerId == id)
                                    .ToListAsync();

            return userSellingBooks;
        }
    }
}
