using AutoMapper;
using BooksApiApp.Data;
using BooksApiApp.DTO;
using BooksApiApp.Model;

namespace BooksApiApp.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<UserSignUpDTO, User>().ReverseMap().MaxDepth(1);

            CreateMap<UserReadOnlyDTO, User>().ReverseMap();    

            CreateMap<UserPatchDTO, User>().ReverseMap();

            CreateMap<UserDTO, User>().ReverseMap();


            CreateMap<BookForSaleReadOnlyDTO, BookForSaleDTO>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
              .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
              .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.BookCategory, opt => opt.MapFrom(src => src.BookCategory))
              .ForMember(dest => dest.SellerId, opt => opt.MapFrom(src => src.Seller.Id))
              .ForMember(dest => dest.ConditionOfBook, opt => opt.MapFrom(src => src.ConditionOfBook))
              .ForMember(dest => dest.TypeOfTransaction, opt => opt.MapFrom(src => src.TypeOfTransaction))
              .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
              .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
              .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));

            CreateMap<BookReadOnlyDTO, Book>().ReverseMap();

            CreateMap<BookForSale, Book>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BookId)) 
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Book.Author))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Book.Description))
                .ForMember(dest => dest.BookCategory, opt => opt.MapFrom(src => src.Book.BookCategory))
                .ForMember(dest => dest.BookForSales, opt => opt.Ignore()) 
                .ForMember(dest => dest.Sales, opt => opt.Ignore()); 

            CreateMap<BookDTO, Book>().ReverseMap();

            CreateMap<BookForSale, BookForSaleDTO>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
              .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
              .ForMember(dest => dest.ConditionOfBook, opt => opt.MapFrom(src => src.ConditionOfBook))
              .ForMember(dest => dest.TypeOfTransaction, opt => opt.MapFrom(src => src.TypeOfTransaction))
              .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
              .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
              .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Book.Title))
              .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Book.Author))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Book.Description))
              .ForMember(dest => dest.BookCategory, opt => opt.MapFrom(src => src.Book.BookCategory))
              .ForMember(dest => dest.SellerId, opt => opt.MapFrom(src => src.SellerId));

            CreateMap<BookForSaleDTO, BookForSale>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.ConditionOfBook, opt => opt.MapFrom(src => src.ConditionOfBook))
                .ForMember(dest => dest.TypeOfTransaction, opt => opt.MapFrom(src => src.TypeOfTransaction))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                .ForMember(dest => dest.SellerId, opt => opt.MapFrom(src => src.SellerId))
                .ForMember(dest => dest.Book, opt => opt.Ignore())              
                .ForMember(dest => dest.Seller, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovalSales, opt => opt.Ignore());

            CreateMap<BookForSale, BookForSaleReadOnlyDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Book.Title))
               .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Book.Author))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Book.Description))
               .ForMember(dest => dest.BookCategory, opt => opt.MapFrom(src => src.Book.BookCategory))
               .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller))
               .ForMember(dest => dest.ConditionOfBook, opt => opt.MapFrom(src => src.ConditionOfBook))
               .ForMember(dest => dest.TypeOfTransaction, opt => opt.MapFrom(src => src.TypeOfTransaction))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
               .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable));

            CreateMap<Book, BookForSaleDTO>()
               .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.BookCategory, opt => opt.MapFrom(src => src.BookCategory))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.ConditionOfBook, opt => opt.Ignore()) 
               .ForMember(dest => dest.TypeOfTransaction, opt => opt.Ignore())
               .ForMember(dest => dest.Price, opt => opt.Ignore())
               .ForMember(dest => dest.PaymentMethod, opt => opt.Ignore())
               .ForMember(dest => dest.SellerId, opt => opt.Ignore())
               .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
               .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<BookForSaleDTO, Book>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BookId))
                    .ForMember(dest => dest.BookCategory, opt => opt.MapFrom(src => src.BookCategory))
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                    .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.BookForSales, opt => opt.Ignore()) 
                    .ForMember(dest => dest.Sales, opt => opt.Ignore());

            CreateMap<Sale, BuyingSellingBooksByIdDTO>()
               .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
               .ForMember(dest => dest.BookCategory, opt => opt.MapFrom(src => src.Book.BookCategory))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Book.Title))
               .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Book.Author))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Book.Description))
               .ForMember(dest => dest.ConditionOfBook, opt => opt.MapFrom(src => src.ConditionOfBook))
               .ForMember(dest => dest.TypeOfTransaction, opt => opt.MapFrom(src => src.TypeOfTransaction))
               .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
               .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer))
               .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller));

            CreateMap<ApprovalSale, BuyingSellingBooksByIdDTO>()
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookForSale.BookId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.BookForSale.Book.Title))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.BookForSale.Book.Author))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.BookForSale.Price))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BookForSale.Book.Description))
            .ForMember(dest => dest.BookCategory, opt => opt.MapFrom(src => src.BookForSale.Book.BookCategory))
            .ForMember(dest => dest.ConditionOfBook, opt => opt.MapFrom(src => src.BookForSale.ConditionOfBook))
            .ForMember(dest => dest.TypeOfTransaction, opt => opt.MapFrom(src => src.BookForSale.TypeOfTransaction))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.BookForSale.PaymentMethod))
            .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer))
            .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.BookForSale.Seller));

            //CreateMap<BookReadyForTrading, BookReadyForTradingDTO>().ReverseMap();

            CreateMap<BookReadyForTrading, BookReadyForTradingDTO>()
             .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
             .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
             .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
             .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
             .ForMember(dest => dest.BookCategory, opt => opt.MapFrom(src => (BookCategory)src.BookCategory))
             .ForMember(dest => dest.ConditionOfBook, opt => opt.MapFrom(src => (ConditionOfBook?)src.ConditionOfBook))
             .ForMember(dest => dest.TypeOfTransaction, opt => opt.MapFrom(src => (TypeOfTransaction)src.TypeOfTransaction))
             .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => (PaymentMethod?)src.PaymentMethod))
             .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer))
             .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller));
        }
    }


}
