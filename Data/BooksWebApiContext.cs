using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BooksApiApp.Data;

public partial class BooksWebApiContext : DbContext
{
    public BooksWebApiContext()
    {
    }

    public BooksWebApiContext(DbContextOptions<BooksWebApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<ApprovalSale> ApprovalSales { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookForSale> BookForSales { get; set; }

    public virtual DbSet<BookReadyForTrading> BookReadyForTradings { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<User> Users { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("ADMIN");

            entity.HasIndex(e => e.UserId, "IX_ADMIN_USER_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");
        });

        modelBuilder.Entity<ApprovalSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_APPROVAL_SALES");

            entity.ToTable("APPROVAL_SALE");

            entity.HasIndex(e => e.BookForSaleId, "IX_APPROVAL_SALE_BOOK_FOR_SALE");

            entity.HasIndex(e => e.BuyerId, "IX_APPROVAL_SALE_BUYER_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApprovalDatetime)
                .HasColumnType("datetime")
                .HasColumnName("APPROVAL_DATETIME");
            entity.Property(e => e.BookForSaleId).HasColumnName("BOOK_FOR_SALE_ID");
            entity.Property(e => e.BuyerId).HasColumnName("BUYER_ID");
            entity.Property(e => e.SubmissionDatetime)
                .HasColumnType("datetime")
                .HasColumnName("SUBMISSION_DATETIME");

            entity.HasOne(d => d.BookForSale).WithMany(p => p.ApprovalSales)
                .HasForeignKey(d => d.BookForSaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_APPROVAL_SALES_BOOK_FOR_SALE");

            entity.HasOne(d => d.Buyer).WithMany(p => p.ApprovalSales)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK_USER_BUYER");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("BOOK");

            entity.HasIndex(e => e.Title, "IX_BOOK");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .HasColumnName("AUTHOR");
            entity.Property(e => e.BookCategory).HasColumnName("BOOK_CATEGORY").HasConversion<int>();
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<BookForSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_BOOK_FOR_SALE");

            entity.ToTable("BOOK_FOR_SALE");

            entity.HasIndex(e => e.Id, "IX_BOOK_FOR_SALE").IsUnique();

            entity.HasIndex(e => e.BookId, "IX_BOOK_FOR_SALE_BOOK_ID");

            entity.HasIndex(e => e.SellerId, "IX_BOOK_FOR_SALE_SELLER_ID");

            entity.HasIndex(e => e.Price, "IX_PRICE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookId).HasColumnName("BOOK_ID");
            entity.Property(e => e.ConditionOfBook).HasColumnName("CONDITION_OF_BOOK").HasConversion<int>();
            entity.Property(e => e.IsAvailable)
                .HasDefaultValue(true)
                .HasColumnName("IS_AVAILABLE");
            entity.Property(e => e.PaymentMethod).HasColumnName("PAYMENT_METHOD").HasConversion<int>();
            entity.Property(e => e.Price)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.SellerId)
                .HasComment("SELLER")
                .HasColumnName("SELLER_ID");
            entity.Property(e => e.TypeOfTransaction).HasColumnName("TYPE_OF_TRANSACTION").HasConversion<int>();

            entity.HasOne(d => d.Book).WithMany(p => p.BookForSales)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOK_FOR_SALE_BOOK");

            entity.HasOne(d => d.Seller).WithMany(p => p.BookForSales)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("FK_USER_SELLER_ID");
        });

        modelBuilder.Entity<BookReadyForTrading>(entity =>
        {
            entity.ToTable("BOOK_READY_FOR_TRADING");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .HasColumnName("AUTHOR");
            entity.Property(e => e.BookCategory).HasColumnName("BOOK_CATEGORY").HasConversion<int>();
            entity.Property(e => e.BookId).HasColumnName("BOOK_ID");
            entity.Property(e => e.BuyerId).HasColumnName("BUYER_ID");
            entity.Property(e => e.ConditionOfBook).HasColumnName("CONDITION_OF_BOOK").HasConversion<int>();
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.PaymentMethod).HasColumnName("PAYMENT_METHOD").HasConversion<int>();
            entity.Property(e => e.Price)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.SellerId).HasColumnName("SELLER_ID");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("TITLE");
            entity.Property(e => e.TypeOfTransaction).HasColumnName("TYPE_OF_TRANSACTION").HasConversion<int>();

            entity.HasOne(d => d.Book).WithMany(p => p.BookReadyForTradings)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_BOOK_READY_FOR_TRADING_BOOK");

            entity.HasOne(d => d.Buyer).WithMany(p => p.BookReadyForTradingBuyers)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOK_READY_FOR_TRADING_USER");

            entity.HasOne(d => d.Seller).WithMany(p => p.BookReadyForTradingSellers)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOK_READY_FOR_TRADING_USER1");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SALES");

            entity.ToTable("SALE");

            entity.HasIndex(e => e.BookId, "IX_BOOK_ID");

            entity.HasIndex(e => e.BuyerId, "IX_SALES_BUYER_ID");

            entity.HasIndex(e => e.SellerId, "IX_SALES_SELLER_ID");

            entity.Property(e => e.Id)
                .HasComment("BUYER")
                .HasColumnName("ID");
            entity.Property(e => e.BookId).HasColumnName("BOOK_ID");
            entity.Property(e => e.BuyerId).HasColumnName("BUYER_ID");
            entity.Property(e => e.ConditionOfBook).HasColumnName("CONDITION_OF_BOOK").HasConversion<int>();
            entity.Property(e => e.PaymentMethod).HasColumnName("PAYMENT_METHOD").HasConversion<int>();
            entity.Property(e => e.Price)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.SellerId).HasColumnName("SELLER_ID");
            entity.Property(e => e.TypeOfTransaction).HasColumnName("TYPE_OF_TRANSACTION").HasConversion<int>();

            entity.HasOne(d => d.Book).WithMany(p => p.Sales)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOK_ID_SALES");

            entity.HasOne(d => d.Buyer).WithMany(p => p.SaleBuyers)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BUYER_USER_ID");

            entity.HasOne(d => d.Seller).WithMany(p => p.SaleSellers)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SELLER_USER_ID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USER");

            entity.HasIndex(e => e.Lastname, "IX_LASTNAME");

            entity.HasIndex(e => e.Username, "IX_USERNAME");

            entity.HasIndex(e => e.Email, "UQ_EMAIL").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ_PHONE_NUMBER").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_USERNAME").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("CITY");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("FIRSTNAME");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("LASTNAME");
            entity.Property(e => e.Number)
                .HasMaxLength(10)
                .HasColumnName("NUMBER");
            entity.Property(e => e.Password)
                .HasMaxLength(512)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(13)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.UserRole).HasColumnName("USER_ROLE").HasConversion<int>();
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("USERNAME");

            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
