
using AutoMapper;
using BooksApiApp.Data;
using BooksApiApp.Repositories;

namespace BooksApiApp.Repositories
{
    /// <summary>
    /// Unit of Work class that provides access to all repositories and handles the transaction management.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BooksWebApiContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public UnitOfWork(BooksWebApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public UserRepository UserRepository => new(_context);

        public BookRepository BookRepository => new(_context);

        public BookForSaleRepository BookForSaleRepository => new(_context);

        public SaleRepository SaleRepository => new(_context);

        public ApprovalSaleRepository ApprovalSaleRepository => new(_context);

        public BookReadyForTradingRepository BookReadyForTradingRepository => new(_context);


        /// <summary>
        /// Saves all changes made in the context to the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation. 
        /// The task result contains a boolean value indicating whether the save operation succeeded.</returns>
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
