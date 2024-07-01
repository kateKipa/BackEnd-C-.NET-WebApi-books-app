using BooksApiApp.Model;
using System.ComponentModel.DataAnnotations;

namespace BooksApiApp.DTO
{
    public class BookReadOnlyDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public BookCategory BookCategory { get; set; }
    }
}
