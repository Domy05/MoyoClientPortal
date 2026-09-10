using System;
using System.Collections.Generic;

namespace ClientPortal.Api.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string Category { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string? Description { get; set; }

    public string? Brand { get; set; }

    public string? PackSize { get; set; }

    public string? Weight { get; set; }

    public string? Unit { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
