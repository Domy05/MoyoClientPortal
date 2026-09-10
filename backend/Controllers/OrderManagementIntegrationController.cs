using ClientPortal.Api.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientPortal.Api.Controllers;

public sealed record OrderStatusUpdateRequest(
    Guid OrderId,
    string Status,
    DateTime? OccurredAtUtc);

[ApiController]
[AllowAnonymous]
[Route("api/integrations/order-management")]
public sealed class OrderManagementIntegrationController : ControllerBase
{
    private readonly IOrderManagementPlatform _platform;
    private readonly IConfiguration _configuration;

    public OrderManagementIntegrationController(
        IOrderManagementPlatform platform,
        IConfiguration configuration)
    {
        _platform = platform;
        _configuration = configuration;
    }

    [HttpPost("status")]
    public async Task<IActionResult> ReceiveStatusUpdate(
        OrderStatusUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var expectedKey = _configuration["OrderManagement:StatusWebhookApiKey"];
        var receivedKey = Request.Headers["X-Order-Management-Key"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(expectedKey) && receivedKey != expectedKey)
        {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(request.Status))
        {
            return BadRequest("Status is required.");
        }

        await _platform.PublishStatusUpdateAsync(
            new OrderStatusUpdatedMessage(
                request.OrderId,
                request.Status,
                request.OccurredAtUtc ?? DateTime.UtcNow),
            cancellationToken);

        return Accepted();
    }
}
