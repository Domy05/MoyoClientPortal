using System.Net.Http.Json;
using ClientPortal.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Api.Integration;

public sealed class OrderManagementWorker : BackgroundService
{
    private readonly IOrderManagementPlatform _platform;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OrderManagementWorker> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public OrderManagementWorker(
        IOrderManagementPlatform platform,
        IServiceScopeFactory scopeFactory,
        ILogger<OrderManagementWorker> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _platform = platform;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _platform.ReadNewOrdersAsync(stoppingToken))
        {
            try
            {
                await ProcessNewOrderAsync(message, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Order-management processing failed for order {OrderId}.",
                    message.OrderId);
            }
        }
    }

    private async Task ProcessNewOrderAsync(
        NewOrderMessage message,
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClientPortalDbContext>();

        var orderExists = await db.Orders
            .AnyAsync(order => order.Id == message.OrderId, cancellationToken);

        if (!orderExists)
        {
            _logger.LogWarning(
                "Order-management received unknown order {OrderId}.",
                message.OrderId);
            return;
        }

        var baseUrl = _configuration["OrderManagement:BaseUrl"];

        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            var client = _httpClientFactory.CreateClient("OrderManagement");
            var response = await client.PostAsJsonAsync(
                new Uri(new Uri(baseUrl), "orders"),
                message,
                cancellationToken);

            response.EnsureSuccessStatusCode();
            return;
        }

        // Local development simulation of the external platform. In production,
        // the configured endpoint receives the queued new-order message.
        await _platform.PublishStatusUpdateAsync(
            new OrderStatusUpdatedMessage(
                message.OrderId,
                "confirmed",
                DateTime.UtcNow),
            cancellationToken);
    }
}

public sealed class OrderStatusUpdateWorker : BackgroundService
{
    private readonly IOrderManagementPlatform _platform;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OrderStatusUpdateWorker> _logger;

    public OrderStatusUpdateWorker(
        IOrderManagementPlatform platform,
        IServiceScopeFactory scopeFactory,
        ILogger<OrderStatusUpdateWorker> logger)
    {
        _platform = platform;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _platform.ReadStatusUpdatesAsync(stoppingToken))
        {
            try
            {
                await ApplyStatusUpdateAsync(message, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Status update processing failed for order {OrderId}.",
                    message.OrderId);
            }
        }
    }

    private async Task ApplyStatusUpdateAsync(
        OrderStatusUpdatedMessage message,
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClientPortalDbContext>();

        var order = await db.Orders
            .FirstOrDefaultAsync(item => item.Id == message.OrderId, cancellationToken);

        if (order is null)
        {
            _logger.LogWarning(
                "Received a status update for unknown order {OrderId}.",
                message.OrderId);
            return;
        }

        order.Status = message.Status;
        await db.SaveChangesAsync(cancellationToken);
    }
}
