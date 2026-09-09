using ClientPortal.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace ClientPortal.Api.Data;

public static class DbSeeder
{
    public static void Seed(ClientPortalDbContext context)
    {
        var random = new Random(42);
        var passwordHasher = new PasswordHasher<Client>();

        // =========================================================
        // CLIENTS
        // =========================================================

        var clients = new List<Client>
        {
            new Client
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@moyo.co.za",
                PhoneNumber = "0821234567",
                CompanyName = "Demo Business",
                IsActive = true
            },

            new Client
            {
                FirstName = "Thabo",
                LastName = "Mokoena",
                Email = "thabo.mokoena@moyo.co.za",
                PhoneNumber = "0821112233",
                CompanyName = "Mokoena Office Solutions",
                IsActive = true
            },

            new Client
            {
                FirstName = "Lerato",
                LastName = "Nkosi",
                Email = "lerato.nkosi@moyo.co.za",
                PhoneNumber = "0832223344",
                CompanyName = "Nkosi Business Services",
                IsActive = true
            },

            new Client
            {
                FirstName = "Sipho",
                LastName = "Dlamini",
                Email = "sipho.dlamini@moyo.co.za",
                PhoneNumber = "0843334455",
                CompanyName = "Dlamini Trading",
                IsActive = true
            },

            new Client
            {
                FirstName = "Amahle",
                LastName = "Ndlovu",
                Email = "amahle.ndlovu@moyo.co.za",
                PhoneNumber = "0724445566",
                CompanyName = "Ndlovu Consulting",
                IsActive = true
            },

            new Client
            {
                FirstName = "Kagiso",
                LastName = "Molefe",
                Email = "kagiso.molefe@moyo.co.za",
                PhoneNumber = "0715556677",
                CompanyName = "Molefe Enterprises",
                IsActive = true
            },

            new Client
            {
                FirstName = "Nomsa",
                LastName = "Khumalo",
                Email = "nomsa.khumalo@moyo.co.za",
                PhoneNumber = "0766667788",
                CompanyName = "Khumalo Holdings",
                IsActive = true
            },

            new Client
            {
                FirstName = "Bongani",
                LastName = "Zulu",
                Email = "bongani.zulu@moyo.co.za",
                PhoneNumber = "0787778899",
                CompanyName = "Zulu Office Group",
                IsActive = true
            },

            new Client
            {
                FirstName = "Zanele",
                LastName = "Mthembu",
                Email = "zanele.mthembu@moyo.co.za",
                PhoneNumber = "0798889900",
                CompanyName = "Mthembu Supplies",
                IsActive = true
            },

            new Client
            {
                FirstName = "Mandla",
                LastName = "Sithole",
                Email = "mandla.sithole@moyo.co.za",
                PhoneNumber = "0811234567",
                CompanyName = "Sithole Projects",
                IsActive = true
            },

            new Client
            {
                FirstName = "Precious",
                LastName = "Mabena",
                Email = "precious.mabena@moyo.co.za",
                PhoneNumber = "0822345678",
                CompanyName = "Mabena Group",
                IsActive = true
            },

            new Client
            {
                FirstName = "Themba",
                LastName = "Masondo",
                Email = "themba.masondo@moyo.co.za",
                PhoneNumber = "0833456789",
                CompanyName = "Masondo Logistics",
                IsActive = true
            },

            new Client
            {
                FirstName = "Ayanda",
                LastName = "Cele",
                Email = "ayanda.cele@moyo.co.za",
                PhoneNumber = "0844567890",
                CompanyName = "Cele Business Partners",
                IsActive = true
            },

            new Client
            {
                FirstName = "Nokuthula",
                LastName = "Mkhize",
                Email = "nokuthula.mkhize@moyo.co.za",
                PhoneNumber = "0725678901",
                CompanyName = "Mkhize Enterprises",
                IsActive = true
            },

            new Client
            {
                FirstName = "Siyabonga",
                LastName = "Shabalala",
                Email = "siyabonga.shabalala@moyo.co.za",
                PhoneNumber = "0716789012",
                CompanyName = "Shabalala Services",
                IsActive = true
            },

            new Client
            {
                FirstName = "Refilwe",
                LastName = "Molefe",
                Email = "refilwe.molefe@moyo.co.za",
                PhoneNumber = "0767890123",
                CompanyName = "Molefe & Associates",
                IsActive = true
            },

            new Client
            {
                FirstName = "Karabo",
                LastName = "Modise",
                Email = "karabo.modise@moyo.co.za",
                PhoneNumber = "0788901234",
                CompanyName = "Modise Corporate Services",
                IsActive = true
            },

            new Client
            {
                FirstName = "Lindiwe",
                LastName = "Maseko",
                Email = "lindiwe.maseko@moyo.co.za",
                PhoneNumber = "0799012345",
                CompanyName = "Maseko Office Supplies",
                IsActive = true
            },

            new Client
            {
                FirstName = "Tshepo",
                LastName = "Mokoena",
                Email = "tshepo.mokoena@moyo.co.za",
                PhoneNumber = "0810123456",
                CompanyName = "Mokoena & Sons",
                IsActive = true
            },

            new Client
            {
                FirstName = "Palesa",
                LastName = "Molefe",
                Email = "palesa.molefe@moyo.co.za",
                PhoneNumber = "0821239876",
                CompanyName = "Molefe Professional Services",
                IsActive = true
            },

            new Client
            {
                FirstName = "Sello",
                LastName = "Radebe",
                Email = "sello.radebe@moyo.co.za",
                PhoneNumber = "0832348765",
                CompanyName = "Radebe Enterprises",
                IsActive = true
            }
        };

        // =========================================================
        // ADD CLIENTS THAT DON'T ALREADY EXIST
        // =========================================================

        foreach (var client in clients)
        {
            var existingClient = context.Clients
                .FirstOrDefault(c => c.Email == client.Email);

            if (existingClient == null)
            {
                client.Id = Guid.NewGuid();
                client.CreatedAt = DateTime.UtcNow;

                client.PasswordHash = passwordHasher.HashPassword(
                    client,
                    "Password123!"
                );

                context.Clients.Add(client);
            }
            else if (existingClient.PasswordHash == "TEMP")
            {
                existingClient.PasswordHash = passwordHasher.HashPassword(
                    existingClient,
                    "Password123!"
                );
            }
        }

        context.SaveChanges();

        // =========================================================
        // GET JOHN DOE
        // =========================================================

        var john = context.Clients
            .First(c => c.Email == "john.doe@moyo.co.za");

        // =========================================================
        // PRODUCTS
        // =========================================================

        var catalog = new (string Key, string DisplayName, string Category)[]
        {
            ("ballpoint-pen", "Ballpoint Pen", "Stationery"),
            ("notebook", "Notebook", "Stationery"),
            ("gel-pens", "Gel Pens", "Stationery"),
            ("copy-paper", "Copy Paper", "Paper"),
            ("office-printer", "Office Printer", "Printing"),
            ("stapler", "Stapler", "Office"),
            ("file-folders", "File Folders", "Office"),
            ("scissors", "Scissors", "Office"),
            ("sticky-notes", "Sticky Notes", "Stationery"),
            ("calculator", "Calculator", "Office"),
            ("paper-clips", "Paper Clips", "Stationery"),
            ("whiteboard-markers", "Whiteboard Markers", "Stationery"),
            ("surface-cleaner", "Surface Cleaner", "Cleaning"),
            ("disinfectant-wipes", "Disinfectant Wipes", "Cleaning"),
            ("facial-tissues", "Facial Tissues", "Cleaning"),
            ("coffee", "Coffee", "Pantry"),
            ("tea-bags", "Tea Bags", "Pantry"),
            ("sugar", "Sugar", "Pantry"),
            ("desk-organiser", "Desk Organiser", "Office"),
            ("printer-ink", "Printer Ink", "Printing")
        };

        // =========================================================
        // INSERT ALL PRODUCT VARIANTS
        // =========================================================

        foreach (var entry in catalog)
        {
            if (!ProductImages.All.TryGetValue(entry.Key, out var variants))
            {
                continue;
            }

            foreach (var variant in variants)
            {
                var productName = $"{variant.Brand} {entry.DisplayName}";

                // Look for this exact brand/product combination.
                var existingProduct = context.Products
                    .FirstOrDefault(p => p.Name == productName);

                if (existingProduct == null)
                {
                    var product = new Product
                    {
                        Id = Guid.NewGuid(),

                        Name = productName,

                        Price = Math.Round(
                            (decimal)(random.NextDouble() * 150 + 10),
                            2
                        ),

                        Stock = random.Next(10, 300),

                        Category = entry.Category,

                        Image = variant.ImageUrl,

                        Description =
                            $"{variant.Brand} {entry.DisplayName.ToLower()} for your office.",

                        Brand = variant.Brand,

                        PackSize = $"{random.Next(1, 20)} units",

                        Weight = $"{random.Next(50, 900)}g",

                        Unit = "pack"
                    };

                    context.Products.Add(product);
                }
                else
                {
                    // Update the existing product with the current
                    // variant information without creating duplicates.

                    existingProduct.Name = productName;
                    existingProduct.Category = entry.Category;
                    existingProduct.Image = variant.ImageUrl;
                    existingProduct.Brand = variant.Brand;
                    existingProduct.Description =
                        $"{variant.Brand} {entry.DisplayName.ToLower()} for your office.";
                }
            }
        }

        context.SaveChanges();

        // Reload all products after inserting the variants.
        var products = context.Products.ToList();

        // =========================================================
        // EXISTING ORDERS
        // =========================================================

        var existingOrders = context.Orders.ToList();

        foreach (var order in existingOrders)
        {
            if (order.ClientId == Guid.Empty)
            {
                order.ClientId = john.Id;
            }
        }

        context.SaveChanges();

        // =========================================================
        // CREATE DEMO ORDERS IF NONE EXIST
        // =========================================================

        if (!context.Orders.Any())
        {
            var statuses = new[]
            {
                "pending",
                "confirmed",
                "shipped",
                "cancelled"
            };

            var allClients = context.Clients.ToList();

            var orders = new List<Order>();

            for (int i = 0; i < 40; i++)
            {
                var client = allClients[i % allClients.Count];

                var product = products[i % products.Count];

                var order = new Order
                {
                    Id = Guid.NewGuid(),

                    ClientId = client.Id,

                    Status = statuses[i % statuses.Length],

                    CreatedAt = DateTime.UtcNow
                        .AddDays(-random.Next(1, 60))
                };

                order.OrderItems.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),

                    OrderId = order.Id,

                    ProductId = product.Id,

                    Quantity = random.Next(1, 10),

                    UnitPrice = product.Price
                });

                orders.Add(order);
            }

            context.Orders.AddRange(orders);

            context.SaveChanges();
        }
    }
}