using Xunit;
using ClientPortal.Api.Controllers;
using ClientPortal.Api.Data;
using ClientPortal.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Api.Tests;

public sealed class CartControllerTests
{
    [Fact]
    public async Task AddToCart_AddsNewItemAndReturnsOk()
    {
        await using var db = CreateDb();
        var client = CreateClient();
        var product = CreateProduct();
        db.Clients.Add(client);
        db.Products.Add(product);
        await db.SaveChangesAsync();

        var controller = new CartController(db);
        var result = await controller.AddToCart(
            client.Id,
            new AddCartItemRequest(product.Id, 2));

        Assert.IsType<OkObjectResult>(result);
        var cartItem = await db.CartItems.SingleAsync();
        Assert.Equal(client.Id, cartItem.ClientId);
        Assert.Equal(product.Id, cartItem.ProductId);
        Assert.Equal(2, cartItem.Quantity);
    }

    [Fact]
    public async Task AddToCart_ExistingItemIncreasesQuantity()
    {
        await using var db = CreateDb();
        var client = CreateClient();
        var product = CreateProduct();
        db.Clients.Add(client);
        db.Products.Add(product);
        db.CartItems.Add(new CartItem
        {
            Id = Guid.NewGuid(),
            ClientId = client.Id,
            ProductId = product.Id,
            Quantity = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();

        var result = await new CartController(db).AddToCart(
            client.Id,
            new AddCartItemRequest(product.Id, 3));

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(5, (await db.CartItems.SingleAsync()).Quantity);
    }

    [Fact]
    public async Task AddToCart_RejectsInvalidQuantityAndMissingEntities()
    {
        await using var db = CreateDb();
        var client = CreateClient();
        db.Clients.Add(client);
        await db.SaveChangesAsync();
        var controller = new CartController(db);

        var invalidQuantity = await controller.AddToCart(
            client.Id,
            new AddCartItemRequest(Guid.NewGuid(), 0));
        var missingProduct = await controller.AddToCart(
            client.Id,
            new AddCartItemRequest(Guid.NewGuid(), 1));
        var missingClient = await controller.AddToCart(
            Guid.NewGuid(),
            new AddCartItemRequest(Guid.NewGuid(), 1));

        Assert.IsType<BadRequestObjectResult>(invalidQuantity);
        Assert.IsType<NotFoundObjectResult>(missingProduct);
        Assert.IsType<NotFoundObjectResult>(missingClient);
    }

    [Fact]
    public async Task CartCanBeUpdatedRemovedAndCleared()
    {
        await using var db = CreateDb();
        var client = CreateClient();
        var product = CreateProduct();
        db.Clients.Add(client);
        db.Products.Add(product);
        db.CartItems.Add(new CartItem
        {
            Id = Guid.NewGuid(),
            ClientId = client.Id,
            ProductId = product.Id,
            Quantity = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();
        var controller = new CartController(db);

        var update = await controller.UpdateCartItem(
            client.Id,
            product.Id,
            new UpdateCartItemRequest(4));
        Assert.IsType<OkObjectResult>(update);
        Assert.Equal(4, (await db.CartItems.SingleAsync()).Quantity);

        var remove = await controller.RemoveFromCart(client.Id, product.Id);
        Assert.IsType<OkObjectResult>(remove);
        Assert.Empty(await db.CartItems.ToListAsync());

        db.CartItems.Add(new CartItem
        {
            Id = Guid.NewGuid(),
            ClientId = client.Id,
            ProductId = product.Id,
            Quantity = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();
        var clear = await controller.ClearCart(client.Id);

        Assert.IsType<OkObjectResult>(clear);
        Assert.Empty(await db.CartItems.ToListAsync());
    }

    private static ClientPortalDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<ClientPortalDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ClientPortalDbContext(options);
    }

    private static Client CreateClient() => new()
    {
        Id = Guid.NewGuid(),
        FirstName = "Test",
        LastName = "Client",
        Email = $"{Guid.NewGuid()}@example.com",
        PasswordHash = "hash",
        PhoneNumber = "0000000000",
        CompanyName = "Test Company",
        IsActive = true,
        CreatedAt = DateTime.UtcNow,
    };

    private static Product CreateProduct() => new()
    {
        Id = Guid.NewGuid(),
        Name = "Notebook",
        Price = 5,
        Stock = 10,
        Category = "Paper",
        Image = "notebook.jpg",
    };
}
