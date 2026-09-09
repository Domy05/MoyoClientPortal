using System;
using System.Collections.Generic;

namespace ClientPortal.Api.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
