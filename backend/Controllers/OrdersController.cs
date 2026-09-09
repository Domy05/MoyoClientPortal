using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientPortal.Api.Data;
using ClientPortal.Api.Models;
using ClientPortal.Api.DTOs;

namespace ClientPortal.Api.Controllers;

public record OrderItemRequest(
Guid ProductId,
int Quantity
);

public record CreateOrderRequest(
List<OrderItemRequest> Items
);

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
private readonly ClientPortalDbContext _context;


public OrdersController(ClientPortalDbContext context)
{
    _context = context;
}

private static OrderDto ToDto(Order order) => new()
{
    Id = order.Id,
    OrderNumber = order.OrderNumber,
    Status = order.Status,
    CreatedAt = order.CreatedAt,

    OrderItems = order.OrderItems.Select(oi => new OrderItemDto
    {
        Id = oi.Id,
        ProductId = oi.ProductId,
        ProductName = oi.Product.Name,
        ProductImage = oi.Product.Image,
        Quantity = oi.Quantity,
        UnitPrice = oi.UnitPrice,
    }).ToList(),
};

[HttpGet]
public async Task<IActionResult> GetOrders()
{
    var orders = await _context.Orders
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync();

    return Ok(orders.Select(ToDto));
}

[HttpGet("{id:guid}")]
public async Task<IActionResult> GetOrder(Guid id)
{
    var order = await _context.Orders
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
        .FirstOrDefaultAsync(o => o.Id == id);

    if (order == null)
        return NotFound("Order not found.");

    return Ok(ToDto(order));
}

[HttpPost("{clientId:guid}")]
public async Task<IActionResult> CreateOrder(
    Guid clientId,
    CreateOrderRequest request)
{
    if (request.Items == null || request.Items.Count == 0)
    {
        return BadRequest("Order must contain at least one item.");
    }

    // Check that the client exists
    var clientExists = await _context.Clients
        .AnyAsync(c => c.Id == clientId);

    if (!clientExists)
    {
        return NotFound("Client not found.");
    }

var order = new Order
{
    Id = Guid.NewGuid(),
    ClientId = clientId,
    OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}",
    Status = "pending",
};

    foreach (var item in request.Items)
    {
        if (item.Quantity <= 0)
        {
            return BadRequest(
                $"Quantity for product {item.ProductId} must be greater than zero."
            );
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == item.ProductId);

        if (product == null)
        {
            return BadRequest(
                $"Product {item.ProductId} not found."
            );
        }

        if (item.Quantity > product.Stock)
        {
            return BadRequest(
                $"Not enough stock for {product.Name}. " +
                $"Only {product.Stock} available."
            );
        }

        order.OrderItems.Add(new OrderItem
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            Quantity = item.Quantity,

            // Always use the price stored in the database
            UnitPrice = product.Price,
        });

        // Reduce stock
        product.Stock -= item.Quantity;
    }

    _context.Orders.Add(order);

    await _context.SaveChangesAsync();

    // Reload products so ToDto can access ProductName and ProductImage
    await _context.Entry(order)
        .Collection(o => o.OrderItems)
        .Query()
        .Include(oi => oi.Product)
        .LoadAsync();

    return CreatedAtAction(
        nameof(GetOrder),
        new { id = order.Id },
        ToDto(order)
    );
}


}
