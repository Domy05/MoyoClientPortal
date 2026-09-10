using Xunit;
using ClientPortal.Api.Controllers;
using ClientPortal.Api.Data;
using ClientPortal.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ClientPortal.Api.Tests;

public sealed class AuthControllerTests
{
    [Fact]
    public async Task Login_WithValidCredentialsReturnsJwtAndClientDetails()
    {
        await using var db = CreateDb();
        var hasher = new PasswordHasher<Client>();
        var client = CreateClient();
        client.PasswordHash = hasher.HashPassword(client, "correct-password");
        db.Clients.Add(client);
        await db.SaveChangesAsync();

        var controller = new AuthController(db, hasher, CreateConfiguration());

        var result = await controller.Login(new LoginRequest
        {
            Email = $"  {client.Email.ToUpperInvariant()} ",
            Password = "correct-password",
        });

        var response = Assert.IsType<OkObjectResult>(result.Result);
        var login = Assert.IsType<LoginResponse>(response.Value);
        Assert.False(string.IsNullOrWhiteSpace(login.Token));
        Assert.Equal(client.Id, login.ClientId);
        Assert.Equal(client.Email, login.Email);
    }

    [Fact]
    public async Task Login_WithUnknownInactiveOrWrongCredentialsReturnsUnauthorized()
    {
        await using var db = CreateDb();
        var hasher = new PasswordHasher<Client>();
        var activeClient = CreateClient();
        activeClient.PasswordHash = hasher.HashPassword(activeClient, "correct-password");
        var inactiveClient = CreateClient();
        inactiveClient.IsActive = false;
        inactiveClient.PasswordHash = hasher.HashPassword(inactiveClient, "correct-password");
        db.Clients.AddRange(activeClient, inactiveClient);
        await db.SaveChangesAsync();
        var controller = new AuthController(db, hasher, CreateConfiguration());

        var unknown = await controller.Login(new LoginRequest
        {
            Email = "missing@example.com",
            Password = "password",
        });
        var inactive = await controller.Login(new LoginRequest
        {
            Email = inactiveClient.Email,
            Password = "correct-password",
        });
        var wrongPassword = await controller.Login(new LoginRequest
        {
            Email = activeClient.Email,
            Password = "wrong-password",
        });

        Assert.IsType<UnauthorizedObjectResult>(unknown.Result);
        Assert.IsType<UnauthorizedObjectResult>(inactive.Result);
        Assert.IsType<UnauthorizedObjectResult>(wrongPassword.Result);
    }

    private static IConfiguration CreateConfiguration() =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "MoyoClientPortalJwtSecretKey2026!Secure",
                ["Jwt:Issuer"] = "MoyoClientPortal",
                ["Jwt:Audience"] = "MoyoClientPortalFrontend",
            })
            .Build();

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
        Email = $"{Guid.NewGuid()}@example.com".ToLowerInvariant(),
        PasswordHash = "placeholder",
        PhoneNumber = "0000000000",
        CompanyName = "Test Company",
        IsActive = true,
        CreatedAt = DateTime.UtcNow,
    };
}
