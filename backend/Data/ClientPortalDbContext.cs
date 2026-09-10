using System;
using System.Collections.Generic;
using ClientPortal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientPortal.Api.Data;

public partial class ClientPortalDbContext : DbContext
{
    public ClientPortalDbContext()
    {
    }

    public ClientPortalDbContext(
        DbContextOptions<ClientPortalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.Property(e => e.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(e => e.CompanyName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())");
        });


        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Orders__3214EC072E139063");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("pending");

            entity.HasOne(e => e.Client)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Orders_Clients");
        });


        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__OrderIte__3214EC07BB8CCD5A");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(10, 2)");

            entity.HasOne(e => e.Order)
                .WithMany(e => e.OrderItems)
                .HasForeignKey(e => e.OrderId)
                .HasConstraintName("FK_OrderItems_Orders");

            entity.HasOne(e => e.Product)
                .WithMany(e => e.OrderItems)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Products__3214EC07DFE5D726");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.Brand)
                .HasMaxLength(100);

            entity.Property(e => e.Category)
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.Image)
                .HasMaxLength(500);

            entity.Property(e => e.Name)
                .HasMaxLength(200);

            entity.Property(e => e.PackSize)
                .HasMaxLength(100);

            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)");

            entity.Property(e => e.Unit)
                .HasMaxLength(50);

            entity.Property(e => e.Weight)
                .HasMaxLength(50);
        });



        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())");

            entity.Property(e => e.Quantity)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasIndex(e => new
            {
                e.ClientId,
                e.ProductId
            })
            .IsUnique();

            entity.HasOne(e => e.Client)
                .WithMany()
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CartItems_Clients");

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CartItems_Products");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}