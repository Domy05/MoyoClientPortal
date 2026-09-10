namespace ClientPortal.Api.Models;

public class LoginResponse
{
    public string Token { get; set; } = null!;

    public Guid ClientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string CompanyName { get; set; } = null!;
}