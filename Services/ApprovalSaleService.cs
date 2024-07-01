using AutoMapper;
using BooksApiApp.Data;
using BooksApiApp.DTO;
using BooksApiApp.Repositories;
using BooksApiApp.Services.Exceptions;

namespace BooksApiApp.Services
{
    public class ApprovalSaleService : IApprovalSaleService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<UserService>? _logger;
        private readonly IMapper? _mapper;

        public ApprovalSaleService(IUnitOfWork? unitOfWork, ILogger<UserService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Requests a purchase asynchronously.
        /// </summary>
        /// <param name="purchaseRequestDTO">The purchase request details.</param>
        /// <returns>True if the purchase request is successful; otherwise, false.</returns>
        public async Task<bool> RequestPurchaseAsync(PurchaseRequestDTO purchaseRequestDTO)
        {
            try
            {
                var bookForSale = await _unitOfWork!.BookForSaleRepository.GetAsync(purchaseRequestDTO.BookForSaleId);
                if (bookForSale == null || !bookForSale.IsAvailable)
                {
                    throw new BookNotFoundException("The book was not found.");
                }

                if (bookForSale.SellerId == purchaseRequestDTO.BuyerId)
                {
                    throw new InvalidOperationException("You cannot purchase your own book.");
                }

                bookForSale.IsAvailable = false;

                var approvalSale = new ApprovalSale
                {
                    BookForSaleId = purchaseRequestDTO.BookForSaleId,
                    BuyerId = purchaseRequestDTO.BuyerId,
                    SubmissionDatetime = DateTime.UtcNow
                };

                await _unitOfWork.ApprovalSaleRepository.AddAsync(approvalSale);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;

            }
        }


        /// <summary>
        /// Retrieves a list of approval sales by the seller ID asynchronously.
        /// </summary>
        /// <param name="sellerId">The seller ID.</param>
        /// <returns>A list of approval sales.</returns>
        public async Task<List<BuyingSellingBooksByIdDTO>> GetApprovalSalesByUserAsync(int sellerId)
        {
            try
            {
                var approvalSales = await _unitOfWork!.ApprovalSaleRepository.GetApprovalSalesBySellerIdAsync(sellerId);

                if (approvalSales == null || !approvalSales.Any())
                {
                    _logger!.LogWarning("No approval sales found for seller with ID {SellerId}", sellerId);
                    return new List<BuyingSellingBooksByIdDTO>(); // Return empty list
                }

                var approvalBooks = _mapper!.Map<List<BuyingSellingBooksByIdDTO>>(approvalSales);

                return approvalBooks;
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Retrieves a list of books that need approval by the buyer ID asynchronously.
        /// </summary>
        /// <param name="buyerId">The buyer ID.</param>
        /// <returns>A list of books that need approval.</returns>
        public async Task<List<BuyingSellingBooksByIdDTO>> GetBooksForNeedApprovalAsync(int buyerId)
        {
            try
            {
                var approvalSales = await _unitOfWork!.ApprovalSaleRepository.GetBooksThatUserAsksForApproveAsync(buyerId);

                if (approvalSales == null || !approvalSales.Any())
                {
                    _logger!.LogWarning("No approval sales found for seller with ID {SellerId}", buyerId);
                    return new List<BuyingSellingBooksByIdDTO>();           // Return empty list
                }

                var approvalBooks = _mapper!.Map<List<BuyingSellingBooksByIdDTO>>(approvalSales);

                return approvalBooks;
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Approves a sale asynchronously.
        /// </summary>
        /// <param name="bookId">The book ID.</param>
        /// <param name="sellerId">The seller ID.</param>
        /// <returns>True if the approval is successful; otherwise, false.</returns>
        public async Task<bool> ApproveSaleAsync(int bookId, int sellerId)
        {
            try
            {
                _logger.LogInformation("Attempting to approve sale for BookId: {BookId}, SellerId: {SellerId}", bookId, sellerId);

                var bookForSale = await _unitOfWork!.BookForSaleRepository.GetBookForSaleByBookIdAndSellerIdAsync(bookId, sellerId);
                if (bookForSale == null)
                {
                    _logger.LogWarning("BookForSale not found for BookId: {BookId}, SellerId: {SellerId}", bookId, sellerId);
                    throw new BookNotFoundException("Book was not found");
                }

                _logger.LogInformation("BookForSale found for BookId: {BookId}, SellerId: {SellerId}", bookId, sellerId);

                var approvalSale = await _unitOfWork.ApprovalSaleRepository.GetApprovalSaleByBookForSaleIdAsync(bookForSale.Id);
                if (approvalSale == null)
                {
                    _logger.LogWarning("ApprovalSale not found for BookForSaleId: {BookForSaleId}", bookForSale.Id);
                    return false;
                }

                _logger.LogInformation("ApprovalSale found for BookForSaleId: {BookForSaleId}", bookForSale.Id);

                var bookReadyForTrading = new BookReadyForTrading
                {
                    BookId = bookForSale.BookId,
                    Title = bookForSale.Book.Title,
                    Author = bookForSale.Book.Author,
                    Price = bookForSale.Price,
                    Description = bookForSale.Book.Description,
                    BookCategory = bookForSale.Book.BookCategory,
                    ConditionOfBook = (int?)bookForSale.ConditionOfBook,
                    TypeOfTransaction = (int)bookForSale.TypeOfTransaction,
                    PaymentMethod = (int?)bookForSale.PaymentMethod,
                    BuyerId = approvalSale.BuyerId,
                    SellerId = bookForSale.SellerId
                };

                _logger.LogInformation("Creating BookReadyForTrading entry for BookId: {BookId}, SellerId: {SellerId}", bookId, sellerId);

                await _unitOfWork.BookReadyForTradingRepository.AddAsync(bookReadyForTrading);

                var deleteResult = await _unitOfWork.ApprovalSaleRepository.DeleteAsync(approvalSale.Id);
                if (!deleteResult)
                {
                    _logger.LogError("Failed to delete approval sale for Id: {ApprovalSaleId}", approvalSale.Id);
                    throw new Exception("Failed to delete approval sale");
                }
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Approved sale for BookId: {BookId}, SellerId: {SellerId}", bookId, sellerId);

                return true;
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Rejects a sale asynchronously.
        /// </summary>
        /// <param name="bookId">The book ID.</param>
        /// <param name="sellerId">The seller ID.</param>
        /// <returns>True if the rejection is successful; otherwise, false.</returns>
        public async Task<bool> RejectSaleAsync(int bookId, int sellerId)
        {
            try
            {
                var bookForSale = await _unitOfWork!.BookForSaleRepository.GetBookForSaleByBookIdAndSellerIdAsync(bookId, sellerId);
                if (bookForSale == null)
                {
                    throw new BookNotFoundException("Book was not found");
                }

                var approvalSale = await _unitOfWork.ApprovalSaleRepository.GetApprovalSaleByBookForSaleIdAsync(bookForSale.Id);
                if (approvalSale == null)
                {
                    return false;
                }

                bookForSale.IsAvailable = true;

                var deleteResult = await _unitOfWork.ApprovalSaleRepository.DeleteAsync(approvalSale.Id);
                if (!deleteResult)
                {
                    throw new Exception("Failed to delete approval sale");
                }

                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }

    }
}
