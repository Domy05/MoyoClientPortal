using ClientPortal.Api.Data;
using ClientPortal.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Api.Controllers;

public record AddCartItemRequest(
    Guid ProductId,
    int Quantity
);

public record UpdateCartItemRequest(
    int Quantity
);

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ClientPortalDbContext _context;

    public CartController(ClientPortalDbContext context)
    {
        _context = context;
    }


    [HttpGet("{clientId}")]
    public async Task<IActionResult> GetCart(Guid clientId)
    {
        var cartItems = await _context.CartItems
            .Where(ci => ci.ClientId == clientId)
            .Include(ci => ci.Product)
            .OrderBy(ci => ci.CreatedAt)
            .ToListAsync();

        var result = cartItems.Select(ci => new
        {
            productId = ci.ProductId,
            name = ci.Product.Name,
            price = ci.Product.Price,
            quantity = ci.Quantity,
            image = ci.Product.Image
        });

        return Ok(result);
    }


    [HttpPost("{clientId}")]
    public async Task<IActionResult> AddToCart(
        Guid clientId,
        AddCartItemRequest request)
    {
        if (request.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than zero.");
        }

        var clientExists = await _context.Clients
            .AnyAsync(c => c.Id == clientId);

        if (!clientExists)
        {
            return NotFound("Client not found.");
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.ProductId);

        if (product == null)
        {
            return NotFound("Product not found.");
        }

        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(ci =>
                ci.ClientId == clientId &&
                ci.ProductId == request.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += request.Quantity;
            existingItem.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Item added to cart."
        });
    }


    [HttpPut("{clientId}/{productId}")]
    public async Task<IActionResult> UpdateCartItem(
        Guid clientId,
        Guid productId,
        UpdateCartItemRequest request)
    {
        if (request.Quantity <= 0)
        {
            return BadRequest("Quantity must be greater than zero.");
        }

        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci =>
                ci.ClientId == clientId &&
                ci.ProductId == productId);

        if (cartItem == null)
        {
            return NotFound("Cart item not found.");
        }

        cartItem.Quantity = request.Quantity;
        cartItem.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Cart item updated."
        });
    }


    [HttpDelete("{clientId}/{productId}")]
    public async Task<IActionResult> RemoveFromCart(
        Guid clientId,
        Guid productId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci =>
                ci.ClientId == clientId &&
                ci.ProductId == productId);

        if (cartItem == null)
        {
            return NotFound("Cart item not found.");
        }

        _context.CartItems.Remove(cartItem);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Item removed from cart."
        });
    }


    [HttpDelete("{clientId}")]
    public async Task<IActionResult> ClearCart(Guid clientId)
    {
        var cartItems = await _context.CartItems
            .Where(ci => ci.ClientId == clientId)
            .ToListAsync();

        if (cartItems.Count > 0)
        {
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        return Ok(new
        {
            message = "Cart cleared."
        });
    }
}
