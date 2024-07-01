namespace BooksApiApp.Services.Exceptions
{
    public class PhonenumberAlreadyExistsException : Exception
    {
        public PhonenumberAlreadyExistsException(string s)
           : base(s)
        {
        }
    }
}
