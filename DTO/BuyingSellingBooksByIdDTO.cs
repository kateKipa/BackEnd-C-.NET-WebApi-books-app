using BooksApiApp.Data;
using BooksApiApp.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BooksApiApp.DTO
{
    public class BuyingSellingBooksByIdDTO
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public BookCategory BookCategory { get; set; }
        public ConditionOfBook? ConditionOfBook { get; set; }
        public TypeOfTransaction TypeOfTransaction { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public User Buyer { get; set; }

        public User Seller { get; set; }
    }
}
