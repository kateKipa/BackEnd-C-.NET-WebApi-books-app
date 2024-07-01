using BooksApiApp.Model;
using System;
using System.Collections.Generic;

namespace BooksApiApp.Data;

public partial class ApprovalSale : IEntity
{
    public int Id { get; set; }

    public int BookForSaleId { get; set; }

    public int BuyerId { get; set; }

    public DateTime? SubmissionDatetime { get; set; }

    public DateTime? ApprovalDatetime { get; set; }

    public virtual BookForSale BookForSale { get; set; } = null!;

    public virtual User Buyer { get; set; } = null!;
}
