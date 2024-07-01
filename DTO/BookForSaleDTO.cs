using BooksApiApp.Model;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;

namespace BooksApiApp.DTO
{
    public class BookForSaleDTO : BookDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title should be between 1 and 200 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Author should be between 1 and 50 characters.")]
        public string Author { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Description should not exceed 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public BookCategory BookCategory { get; set; }
        public ConditionOfBook? ConditionOfBook { get; set; }

        [Required(ErrorMessage = "Type of transaction is required.")]
        public TypeOfTransaction TypeOfTransaction { get; set; }

        [Range(0.00, 9999.99, ErrorMessage = "Price must be between 0.00 and 9999.99.")]
        public decimal Price { get; set; }
        
        public PaymentMethod? PaymentMethod { get; set; }

        public int SellerId { get; set; }     // This will be set by the controller

        public bool IsAvailable { get; set; } = true;

    }
}
