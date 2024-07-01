using BooksApiApp.Model;
using System;
using System.Collections.Generic;

namespace BooksApiApp.Data;

public partial class Sale : IEntity
{

    public int Id { get; set; }

    public int BookId { get; set; }

    public int SellerId { get; set; }

    public int BuyerId { get; set; }

    public decimal Price { get; set; }

    public int? PaymentMethod { get; set; }

    public int TypeOfTransaction { get; set; }

    public int? ConditionOfBook { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User Buyer { get; set; } = null!;

    public virtual User Seller { get; set; } = null!;
}
