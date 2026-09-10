using Xunit;
using ClientPortal.Api.Controllers;
using ClientPortal.Api.DTOs;
using ClientPortal.Api.Integration;
using Microsoft.AspNetCore.Mvc;

namespace ClientPortal.Api.Tests;

public sealed class ProductsControllerTests
{
    [Fact]
    public async Task GetProducts_ReturnsProductsFromProductSystem()
    {
        var products = new List<ProductDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Notebook",
                Price = 5,
                Stock = 10,
                Category = "Paper",
                Image = "notebook.jpg",
            },
        };
        var controller = new ProductsController(new FakeProductSystemClient(products));

        var result = await controller.GetProducts(CancellationToken.None);

        var response = Assert.IsType<OkObjectResult>(result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(response.Value);
        Assert.Single(returnedProducts);
        Assert.Equal("Notebook", returnedProducts.Single().Name);
    }

    [Fact]
    public async Task GetProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var controller = new ProductsController(new FakeProductSystemClient([]));

        var result = await controller.GetProduct(Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    private sealed class FakeProductSystemClient : IProductSystemClient
    {
        private readonly IReadOnlyList<ProductDto> _products;

        public FakeProductSystemClient(IReadOnlyList<ProductDto> products)
        {
            _products = products;
        }

        public Task<IReadOnlyList<ProductDto>> GetProductsAsync(
            CancellationToken cancellationToken = default) =>
            Task.FromResult(_products);

        public Task<ProductDto?> GetProductAsync(
            Guid id,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(_products.FirstOrDefault(product => product.Id == id));
    }
}
