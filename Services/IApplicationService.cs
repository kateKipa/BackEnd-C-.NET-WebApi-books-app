namespace BooksApiApp.Services
{
    public interface IApplicationService
    {
        UserService UserService { get; }
        BookService BookService { get; }
        BookForSaleService BookForSaleService { get; }
        SaleService SaleService { get; }
        ApprovalSaleService ApprovalSaleService { get; }

        BookReadyForTradingService BookReadyForTradingService { get; }

    }
}
