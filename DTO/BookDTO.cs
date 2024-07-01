using BooksApiApp.Model;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BooksApiApp.DTO
{
    public class BookDTO
    {
        public int Id { get; set; }

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

    }
}
