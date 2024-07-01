namespace BooksApiApp.Services.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException(string s)
            : base(s)
        {
        }
    }
}
