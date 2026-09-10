using System.Threading.Channels;

namespace ClientPortal.Api.Integration;

public sealed record NewOrderMessage(
    Guid OrderId,
    Guid ClientId,
    DateTime OccurredAtUtc);

public sealed record OrderStatusUpdatedMessage(
    Guid OrderId,
    string Status,
    DateTime OccurredAtUtc);

public interface IOrderManagementPlatform
{
    ValueTask PublishNewOrderAsync(
        NewOrderMessage message,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<NewOrderMessage> ReadNewOrdersAsync(
        CancellationToken cancellationToken = default);

    ValueTask PublishStatusUpdateAsync(
        OrderStatusUpdatedMessage message,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<OrderStatusUpdatedMessage> ReadStatusUpdatesAsync(
        CancellationToken cancellationToken = default);
}

public sealed class InMemoryOrderManagementPlatform : IOrderManagementPlatform
{
    private readonly Channel<NewOrderMessage> _newOrders =
        Channel.CreateUnbounded<NewOrderMessage>();

    private readonly Channel<OrderStatusUpdatedMessage> _statusUpdates =
        Channel.CreateUnbounded<OrderStatusUpdatedMessage>();

    public ValueTask PublishNewOrderAsync(
        NewOrderMessage message,
        CancellationToken cancellationToken = default)
    {
        return _newOrders.Writer.WriteAsync(message, cancellationToken);
    }

    public IAsyncEnumerable<NewOrderMessage> ReadNewOrdersAsync(
        CancellationToken cancellationToken = default)
    {
        return _newOrders.Reader.ReadAllAsync(cancellationToken);
    }

    public ValueTask PublishStatusUpdateAsync(
        OrderStatusUpdatedMessage message,
        CancellationToken cancellationToken = default)
    {
        return _statusUpdates.Writer.WriteAsync(message, cancellationToken);
    }

    public IAsyncEnumerable<OrderStatusUpdatedMessage> ReadStatusUpdatesAsync(
        CancellationToken cancellationToken = default)
    {
        return _statusUpdates.Reader.ReadAllAsync(cancellationToken);
    }
}
