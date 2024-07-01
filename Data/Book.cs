using BooksApiApp.Model;
using System;
using System.Collections.Generic;

namespace BooksApiApp.Data;

public partial class Book : IEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Description { get; set; }

    public int BookCategory { get; set; }

    public virtual ICollection<BookForSale> BookForSales { get; set; } = new List<BookForSale>();

    public virtual ICollection<BookReadyForTrading> BookReadyForTradings { get; set; } = new List<BookReadyForTrading>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
