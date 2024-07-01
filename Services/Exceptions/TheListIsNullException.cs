namespace BooksApiApp.Services.Exceptions
{
    public class TheListIsNullException : Exception
    {
        public TheListIsNullException(string s)
            : base(s)
        {
        }
    }
}
