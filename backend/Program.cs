using System.Text;
using ClientPortal.Api.Data;
using ClientPortal.Api.Integration;
using ClientPortal.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ClientPortalDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<IPasswordHasher<Client>, PasswordHasher<Client>>();
builder.Services.AddHttpClient("ProductSystem");
builder.Services.AddHttpClient("OrderManagement");
builder.Services.AddScoped<IProductSystemClient, ProductSystemClient>();
builder.Services.AddSingleton<IOrderManagementPlatform, InMemoryOrderManagementPlatform>();
builder.Services.AddHostedService<OrderManagementWorker>();
builder.Services.AddHostedService<OrderStatusUpdateWorker>();

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("JWT signing key is not configured.");
}

const string localJwtScheme = "LocalJwt";

var authentication = builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = localJwtScheme;
        options.DefaultChallengeScheme = localJwtScheme;
    })
    .AddJwtBearer(localJwtScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

var oidcAuthority = builder.Configuration["Authentication:Oidc:Authority"];
var oidcAudience = builder.Configuration["Authentication:Oidc:Audience"];

if (!string.IsNullOrWhiteSpace(oidcAuthority))
{
    authentication.AddJwtBearer("Oidc", options =>
    {
        options.Authority = oidcAuthority;
        options.Audience = oidcAudience;
        options.RequireHttpsMetadata = true;
    });
}

builder.Services.AddAuthorization(options =>
{
    var schemes = string.IsNullOrWhiteSpace(oidcAuthority)
        ? new[] { localJwtScheme }
        : new[] { localJwtScheme, "Oidc" };

    options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder(schemes)
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ClientPortalDbContext>();
    DbSeeder.Seed(db);
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
