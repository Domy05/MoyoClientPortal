using System.Net.Http.Json;
using ClientPortal.Api.Data;
using ClientPortal.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Api.Integration;

public interface IProductSystemClient
{
    Task<IReadOnlyList<ProductDto>> GetProductsAsync(
        CancellationToken cancellationToken = default);

    Task<ProductDto?> GetProductAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}

public sealed class ProductSystemClient : IProductSystemClient
{
    private readonly ClientPortalDbContext _db;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ProductSystemClient(
        ClientPortalDbContext db,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _db = db;
        _httpClient = httpClientFactory.CreateClient("ProductSystem");
        _configuration = configuration;
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync(
        CancellationToken cancellationToken = default)
    {
        var baseUrl = _configuration["ProductSystem:BaseUrl"];

        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            var products = await _httpClient.GetFromJsonAsync<List<ProductDto>>(
                new Uri(new Uri(baseUrl), "products"),
                cancellationToken);

            return products ?? [];
        }

        return await _db.Products
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                Image = product.Image,
                Description = product.Description,
                Brand = product.Brand,
                PackSize = product.PackSize,
                Weight = product.Weight,
                Unit = product.Unit
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductDto?> GetProductAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var baseUrl = _configuration["ProductSystem:BaseUrl"];

        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>(
                new Uri(new Uri(baseUrl), $"products/{id}"),
                cancellationToken);
        }

        return await _db.Products
            .Where(product => product.Id == id)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                Image = product.Image,
                Description = product.Description,
                Brand = product.Brand,
                PackSize = product.PackSize,
                Weight = product.Weight,
                Unit = product.Unit
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
