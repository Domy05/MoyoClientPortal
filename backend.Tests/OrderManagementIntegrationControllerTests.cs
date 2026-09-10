using Xunit;
using ClientPortal.Api.Controllers;
using ClientPortal.Api.Integration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ClientPortal.Api.Tests;

public sealed class OrderManagementIntegrationControllerTests
{
    [Fact]
    public async Task ReceiveStatusUpdate_WithValidKeyPublishesMessage()
    {
        var platform = new FakeOrderManagementPlatform();
        var controller = CreateController(platform, "secret-key");
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext(),
        };
        controller.Request.Headers["X-Order-Management-Key"] = "secret-key";
        var orderId = Guid.NewGuid();

        var result = await controller.ReceiveStatusUpdate(
            new OrderStatusUpdateRequest(orderId, "shipped", null),
            CancellationToken.None);

        Assert.IsType<AcceptedResult>(result);
        var message = Assert.Single(platform.StatusUpdates);
        Assert.Equal(orderId, message.OrderId);
        Assert.Equal("shipped", message.Status);
    }

    [Fact]
    public async Task ReceiveStatusUpdate_RejectsInvalidKeyOrMissingStatus()
    {
        var platform = new FakeOrderManagementPlatform();
        var controller = CreateController(platform, "secret-key");
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext(),
        };
        controller.Request.Headers["X-Order-Management-Key"] = "wrong-key";

        var unauthorized = await controller.ReceiveStatusUpdate(
            new OrderStatusUpdateRequest(Guid.NewGuid(), "shipped", null),
            CancellationToken.None);
        Assert.IsType<UnauthorizedResult>(unauthorized);

        controller.Request.Headers["X-Order-Management-Key"] = "secret-key";
        var invalidStatus = await controller.ReceiveStatusUpdate(
            new OrderStatusUpdateRequest(Guid.NewGuid(), "", null),
            CancellationToken.None);
        Assert.IsType<BadRequestObjectResult>(invalidStatus);
    }

    private static OrderManagementIntegrationController CreateController(
        FakeOrderManagementPlatform platform,
        string apiKey) =>
        new(
            platform,
            new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["OrderManagement:StatusWebhookApiKey"] = apiKey,
                })
                .Build());

    private sealed class FakeOrderManagementPlatform : IOrderManagementPlatform
    {
        public List<OrderStatusUpdatedMessage> StatusUpdates { get; } = [];

        public ValueTask PublishNewOrderAsync(
            NewOrderMessage message,
            CancellationToken cancellationToken = default) =>
            ValueTask.CompletedTask;

        public async IAsyncEnumerable<NewOrderMessage> ReadNewOrdersAsync(
            [System.Runtime.CompilerServices.EnumeratorCancellation]
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            yield break;
        }

        public ValueTask PublishStatusUpdateAsync(
            OrderStatusUpdatedMessage message,
            CancellationToken cancellationToken = default)
        {
            StatusUpdates.Add(message);
            return ValueTask.CompletedTask;
        }

        public async IAsyncEnumerable<OrderStatusUpdatedMessage> ReadStatusUpdatesAsync(
            [System.Runtime.CompilerServices.EnumeratorCancellation]
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            foreach (var statusUpdate in StatusUpdates)
            {
                yield return statusUpdate;
            }
        }
    }
}
