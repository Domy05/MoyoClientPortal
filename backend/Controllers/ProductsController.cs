using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientPortal.Api.Data;
using ClientPortal.Api.DTOs;

namespace ClientPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ClientPortalDbContext _context;

    public ProductsController(ClientPortalDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category,
                Image = p.Image,
                Description = p.Description,
                Brand = p.Brand,
                PackSize = p.PackSize,
                Weight = p.Weight,
                Unit = p.Unit,
            })
            .ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await _context.Products
            .Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category,
                Image = p.Image,
                Description = p.Description,
                Brand = p.Brand,
                PackSize = p.PackSize,
                Weight = p.Weight,
                Unit = p.Unit,
            })
            .FirstOrDefaultAsync();
        if (product == null) return NotFound();
        return Ok(product);
    }
}