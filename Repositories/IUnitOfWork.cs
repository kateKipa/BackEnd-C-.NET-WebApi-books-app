namespace BooksApiApp.Repositories
{
    public interface IUnitOfWork
    {
        public UserRepository UserRepository { get; }
        public BookRepository BookRepository { get; }
        public BookForSaleRepository BookForSaleRepository { get; }
        public SaleRepository SaleRepository { get; }

        public ApprovalSaleRepository ApprovalSaleRepository { get; }

        public BookReadyForTradingRepository BookReadyForTradingRepository { get; }

        Task<bool> SaveAsync();
    }
}
