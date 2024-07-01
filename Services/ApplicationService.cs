using AutoMapper;
using BooksApiApp.Repositories;

namespace BooksApiApp.Services
{
    /// <summary>
    /// Application service class that provides access to various services within the application.
    /// </summary>
    public class ApplicationService : IApplicationService
    {
        protected readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;
        private readonly ILogger<UserService>? _logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="logger">The logger instance.</param>
        public ApplicationService(IUnitOfWork? unitOfWork, IMapper? mapper, ILogger<UserService>? logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            
        }

        public UserService UserService => new(_unitOfWork, _logger, _mapper);
        public BookService BookService => new(_unitOfWork, _logger, _mapper);

        public BookForSaleService BookForSaleService => new(_unitOfWork, _logger, _mapper);

        public SaleService SaleService => new(_unitOfWork, _logger, _mapper);

        public ApprovalSaleService ApprovalSaleService => new(_unitOfWork, _logger, _mapper);

        public BookReadyForTradingService BookReadyForTradingService => new(_unitOfWork, _logger, _mapper);
    }
}
