using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClientPortal.Api.Data;
using ClientPortal.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ClientPortal.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ClientPortalDbContext _context;
    private readonly IPasswordHasher<Client> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthController(
        ClientPortalDbContext context,
        IPasswordHasher<Client> passwordHasher,
        IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Email == email);

        if (client == null || !client.IsActive)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var passwordResult = _passwordHasher.VerifyHashedPassword(
            client,
            client.PasswordHash,
            request.Password
        );

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var token = GenerateToken(client);

        return Ok(new LoginResponse
        {
            Token = token,
            ClientId = client.Id,
            FirstName = client.FirstName,
            LastName = client.LastName,
            Email = client.Email,
            CompanyName = client.CompanyName
        });
    }

    private string GenerateToken(Client client)
    {
        var key = _configuration["Jwt:Key"];

        var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                client.Id.ToString()
            ),

            new Claim(
                ClaimTypes.Email,
                client.Email
            ),

            new Claim(
                ClaimTypes.Name,
                $"{client.FirstName} {client.LastName}"
            )
        };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key!)
        );

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}