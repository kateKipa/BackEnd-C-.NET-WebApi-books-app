using BooksApiApp.Model;
using System;
using System.Collections.Generic;

namespace BooksApiApp.Data;

public partial class BookReadyForTrading : IEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Description { get; set; }

    public int BookCategory { get; set; }

    public decimal Price { get; set; }

    public int? ConditionOfBook { get; set; }

    public int TypeOfTransaction { get; set; }

    public int? PaymentMethod { get; set; }

    public int BuyerId { get; set; }

    public int SellerId { get; set; }

    public int BookId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User Buyer { get; set; } = null!;

    public virtual User Seller { get; set; } = null!;
}
