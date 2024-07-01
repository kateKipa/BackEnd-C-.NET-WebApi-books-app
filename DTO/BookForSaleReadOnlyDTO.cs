using BooksApiApp.Model;
using System.ComponentModel.DataAnnotations;

namespace BooksApiApp.DTO
{
    public class BookForSaleReadOnlyDTO : BookReadOnlyDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public BookCategory BookCategory { get; set; }
        public ConditionOfBook? ConditionOfBook { get; set; }

        public TypeOfTransaction TypeOfTransaction { get; set; }

        public decimal Price { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }

        public UserReadOnlyDTO Seller { get; set; }
        public bool IsAvailable { get; set; } = true;


    }
}
