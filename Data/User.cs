using BooksApiApp.Model;
using System;
using System.Collections.Generic;

namespace BooksApiApp.Data;

public partial class User : IEntity
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int UserRole { get; set; }

    public string? City { get; set; }

    public string? Address { get; set; }

    public string? Number { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ApprovalSale> ApprovalSales { get; set; } = new List<ApprovalSale>();

    public virtual ICollection<BookForSale> BookForSales { get; set; } = new List<BookForSale>();

    public virtual ICollection<BookReadyForTrading> BookReadyForTradingBuyers { get; set; } = new List<BookReadyForTrading>();

    public virtual ICollection<BookReadyForTrading> BookReadyForTradingSellers { get; set; } = new List<BookReadyForTrading>();

    public virtual ICollection<Sale> SaleBuyers { get; set; } = new List<Sale>();

    public virtual ICollection<Sale> SaleSellers { get; set; } = new List<Sale>();
}
