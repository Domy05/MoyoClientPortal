using Xunit;
using ClientPortal.Api.Controllers;
using ClientPortal.Api.Data;
using ClientPortal.Api.Integration;
using ClientPortal.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Api.Tests;

public sealed class OrdersControllerTests
{
    [Fact]
    public async Task GetOrders_ReturnsOrdersWithProductDetails()
    {
        await using var db = CreateDb();
        var product = CreateProduct();
        var order = CreateOrder();
        order.OrderItems.Add(new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            Order = order,
            ProductId = product.Id,
            Product = product,
            Quantity = 2,
            UnitPrice = product.Price,
        });
        db.Products.Add(product);
        db.Orders.Add(order);
        await db.SaveChangesAsync();

        var controller = new OrdersController(db, new FakeOrderManagementPlatform());

        var result = await controller.GetOrders();

        var response = Assert.IsType<OkObjectResult>(result);
        var orders = Assert.IsAssignableFrom<IEnumerable<ClientPortal.Api.DTOs.OrderDto>>(response.Value);
        var returnedOrder = Assert.Single(orders);
        var returnedItem = Assert.Single(returnedOrder.OrderItems);
        Assert.Equal(product.Name, returnedItem.ProductName);
        Assert.Equal(2, returnedItem.Quantity);
    }

    [Fact]
    public async Task CreateOrder_WithNoItems_ReturnsBadRequest()
    {
        await using var db = CreateDb();
        var controller = new OrdersController(db, new FakeOrderManagementPlatform());

        var result = await controller.CreateOrder(
            Guid.NewGuid(),
            new CreateOrderRequest([]));

        var response = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Order must contain at least one item.", response.Value);
    }

    [Fact]
    public async Task CreateOrder_CreatesOrderDecreasesStockAndPublishesMessage()
    {
        await using var db = CreateDb();
        var client = CreateClient();
        var product = CreateProduct(stock: 10);
        var orderManagement = new FakeOrderManagementPlatform();
        db.Clients.Add(client);
        db.Products.Add(product);
        await db.SaveChangesAsync();

        var controller = new OrdersController(db, orderManagement);
        var result = await controller.CreateOrder(
            client.Id,
            new CreateOrderRequest([
                new OrderItemRequest(product.Id, 3),
            ]));

        var response = Assert.IsType<CreatedAtActionResult>(result);
        var returnedOrder = Assert.IsType<ClientPortal.Api.DTOs.OrderDto>(response.Value);
        var storedProduct = await db.Products.SingleAsync(item => item.Id == product.Id);
        var storedOrder = await db.Orders.Include(item => item.OrderItems)
            .SingleAsync(item => item.Id == returnedOrder.Id);

        Assert.Equal(7, storedProduct.Stock);
        Assert.Single(storedOrder.OrderItems);
        Assert.Equal(3, storedOrder.OrderItems.Single().Quantity);
        var message = Assert.Single(orderManagement.PublishedOrders);
        Assert.Equal(returnedOrder.Id, message.OrderId);
        Assert.Equal(client.Id, message.ClientId);
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

    private static Product CreateProduct(int stock = 10) => new()
    {
        Id = Guid.NewGuid(),
        Name = "Notebook",
        Price = 5,
        Stock = stock,
        Category = "Paper",
        Image = "notebook.jpg",
    };

    private static Order CreateOrder() => new()
    {
        Id = Guid.NewGuid(),
        ClientId = Guid.NewGuid(),
        OrderNumber = "ORD-TEST-001",
        Status = "pending",
        CreatedAt = DateTime.UtcNow,
    };

    private sealed class FakeOrderManagementPlatform : IOrderManagementPlatform
    {
        public List<NewOrderMessage> PublishedOrders { get; } = [];
        public List<OrderStatusUpdatedMessage> PublishedStatusUpdates { get; } = [];

        public ValueTask PublishNewOrderAsync(
            NewOrderMessage message,
            CancellationToken cancellationToken = default)
        {
            PublishedOrders.Add(message);
            return ValueTask.CompletedTask;
        }

        public async IAsyncEnumerable<NewOrderMessage> ReadNewOrdersAsync(
            [System.Runtime.CompilerServices.EnumeratorCancellation]
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            foreach (var message in PublishedOrders)
            {
                yield return message;
            }
        }

        public ValueTask PublishStatusUpdateAsync(
            OrderStatusUpdatedMessage message,
            CancellationToken cancellationToken = default)
        {
            PublishedStatusUpdates.Add(message);
            return ValueTask.CompletedTask;
        }

        public async IAsyncEnumerable<OrderStatusUpdatedMessage> ReadStatusUpdatesAsync(
            [System.Runtime.CompilerServices.EnumeratorCancellation]
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            foreach (var message in PublishedStatusUpdates)
            {
                yield return message;
            }
        }
    }
}
