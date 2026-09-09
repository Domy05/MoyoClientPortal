using System;

namespace ClientPortal.Api.Models;

public class CartItem
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}