using ClientPortal.Api.Models;

namespace ClientPortal.Api.Data;

public static class DbSeeder
{
    public static void Seed(ClientPortalDbContext context)
    {
        if (context.Products.Any())
        {
            return; // already seeded
        }

        var categories = new[] { "Stationery", "Paper", "Printing", "Office", "Cleaning", "Pantry" };
        var random = new Random(42);

        var products = new List<Product>();
        for (int i = 1; i <= 20; i++)
        {
            var category = categories[(i - 1) % categories.Length];
            products.Add(new Product
            {
                Name = $"{category} Item {i}",
                Price = Math.Round((decimal)(random.NextDouble() * 150 + 10), 2),
                Stock = random.Next(10, 300),
                Category = category,
                Image = $"https://placehold.co/500x400/e8e2d0/2f4f3f?text={category}+{i}",
                Description = $"Sample {category.ToLower()} product number {i} for demo purposes.",
                Brand = "Moyo",
                PackSize = $"{random.Next(1, 20)} units",
                Weight = $"{random.Next(50, 900)}g",
                Unit = "pack",
            });
        }

        context.Products.AddRange(products);
        context.SaveChanges(); // save first so products have real Ids

        var statuses = new[] { "pending", "confirmed", "shipped", "cancelled" };
        var orders = new List<Order>();

        for (int i = 0; i < 20; i++)
        {
            var product = products[i % products.Count];
            var order = new Order
            {
                Status = statuses[i % statuses.Length],
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 60)),
            };

            order.OrderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = random.Next(1, 10),
                UnitPrice = product.Price,
            });

            orders.Add(order);
        }

        context.Orders.AddRange(orders);
        context.SaveChanges();
    }
}