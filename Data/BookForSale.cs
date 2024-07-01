using BooksApiApp.Model;
using System;
using System.Collections.Generic;

namespace BooksApiApp.Data;

public partial class BookForSale : IEntity
{
    public int Id { get; set; }

    /// <summary>
    /// SELLER
    /// </summary>
    public int SellerId { get; set; }

    public int BookId { get; set; }

    public decimal Price { get; set; }

    public int? ConditionOfBook { get; set; }

    public int TypeOfTransaction { get; set; }

    public int? PaymentMethod { get; set; }

    public bool IsAvailable { get; set; }

    public virtual ICollection<ApprovalSale> ApprovalSales { get; set; } = new List<ApprovalSale>();

    public virtual Book Book { get; set; } = null!;

    public virtual User Seller { get; set; } = null!;
}
