using Microsoft.AspNetCore.Identity;

namespace Online_Shop.Models
{
    public class DbInitializer
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            if (!await roleManager.RoleExistsAsync(Roles.Seller))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Seller));
            }

            if (!await roleManager.RoleExistsAsync(Roles.Customer))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Customer));
            }
        }
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    IsActive = true
                };

                var result = await userManager.CreateAsync(adminUser, "OnlineShopAdmin1");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Roles.Admin);
                }
            }
        }
        public static async Task SeedSellersAsync(UserManager<ApplicationUser> userManager)
        {
            var sellers = await userManager.GetUsersInRoleAsync(Roles.Seller);

            if (sellers == null || sellers.Count == 0)
            {
                var seller1 = new ApplicationUser
                {
                    UserName = "Apple",
                    Email = "apple@example.com",
                    IsActive = true
                };
                var result1 = await userManager.CreateAsync(seller1, "Apple1");

                var seller2 = new ApplicationUser
                {
                    UserName = "Samsung",
                    Email = "samsung@example.com",
                    IsActive = true
                };
                var result2 = await userManager.CreateAsync(seller2, "Samsung1");

                var seller3 = new ApplicationUser
                {
                    UserName = "Lenovo",
                    Email = "lenovo@example.com",
                    IsActive = true
                };
                var result3 = await userManager.CreateAsync(seller3, "Lenovo1");

                if (result1.Succeeded && result2.Succeeded && result3.Succeeded)
                {
                    await userManager.AddToRoleAsync(seller1, Roles.Seller);
                    await userManager.AddToRoleAsync(seller2, Roles.Seller);
                    await userManager.AddToRoleAsync(seller3, Roles.Seller);
                }
            }
        }
        public static void SeedCartStatus(ApplicationDbContext context)
        {
            if (!context.CartStatuses.Any())
            {
                var cartStatuses = new List<CartStatus>
                {
                    new CartStatus { StatusName = Statuses.InProgress },
                    new CartStatus { StatusName = Statuses.Completed }
                };

                context.CartStatuses.AddRange(cartStatuses);
                context.SaveChanges();
            }
        }
        public static void SeedCategories(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Mobile Phones" },
                    new Category { Name = "Laptops" },
                    new Category { Name = "Televisions" },
                    new Category { Name = "Refrigerator" },
                    new Category { Name = "Juice" },
                    new Category { Name = "Tablets" },
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }
        public static async Task SeedProductsAsync(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            List<ApplicationUser> sellers = (List<ApplicationUser>)await userManager.GetUsersInRoleAsync(Roles.Seller);

            if (!context.Products.Any())
            {
                ApplicationUser Apple = sellers.FirstOrDefault(u => u.UserName == "Apple");
                ApplicationUser Lenovo = sellers.FirstOrDefault(u => u.UserName == "Lenovo");
                ApplicationUser Samasung = sellers.FirstOrDefault(u => u.UserName == "Samsung");

                var products = new List<Product>
                {
                    new Product{
                        Name = "Iphone14",
                        Image = "iphone14.jpg",
                        Price = 1000,
                        Description = "Apple iphone 14, Single SIM, 128Gb, 4Gb Ram, 5G - Midnight",
                        IsActive = true,
                        StockQuantity = 5,
                        CategoryId = 1,
                        SellerId = Apple.Id
                    },
                    new Product{
                        Name = "Iphone13 blue",
                        Image = "Iphone13-blue.jpeg",
                        Price = 1200,
                        Description = "Apple iPhone 13, 128GB, 4GB RAM, 5G - Blue (Japanese Version)",
                        IsActive = true,
                        StockQuantity = 6,
                        CategoryId = 1,
                        SellerId = Apple.Id
                    },
                    new Product{
                        Name = "Iphone14 Pro Max",
                        Image = "Iphone14ProMax.jpeg",
                        Price = 2100,
                        Description = "Apple iPhone 14 Pro Max Dual SIM, 256GB, 6GB RAM, 5G - Deep Purple Without Warranty",
                        IsActive = true,
                        StockQuantity = 10,
                        CategoryId = 1,
                        SellerId = Apple.Id
                    },
                    new Product{
                        Name = "samsung 43 4k smart led tv",
                        Image = "samsung-43-4k-smart-led-tv.jpg",
                        Price = 370,
                        Description = "Samsung 43 Inch 4K UHD Smart LED TV with Built-in Receiver - 43CU7000",
                        IsActive = true,
                        StockQuantity = 5,
                        CategoryId = 3,
                        SellerId = Samasung.Id
                    },
                    new Product{
                        Name = "Samsung Galaxy A14",
                        Image = "Samsung-Galaxy-A14.jpeg",
                        Price = 200,
                        Description = "Samsung Galaxy A14 Dual SIM, 128GB, 4GB RAM, 4G LTE, Black - Local Version",
                        IsActive = true,
                        StockQuantity = 5,
                        CategoryId = 1,
                        SellerId = Samasung.Id
                    },
                    new Product{
                        Name = "Samsung Series 8",
                        Image = "Samsung-Series-8.png",
                        Price = 420,
                        Description = "Samsung 50 Inch 4K UHD Smart LED TV with Built in Receiver - 50BU8000",
                        IsActive = true,
                        StockQuantity = 5,
                        CategoryId = 3,
                        SellerId = Samasung.Id
                    },
                    new Product{
                        Name = "Lenovo Ideapad 1",
                        Image = "Lenovo Ideapad 1.jpeg",
                        Price = 340,
                        Description = "Lenovo Ideapad 1 Laptop, Intel Celeron N4020, 15.6 Inch FHD, 256GB SSD, 8GB RAM, Intel HD Graphics, Windows 11 - Blue",
                        IsActive = true,
                        StockQuantity = 5,
                        CategoryId = 2,
                        SellerId = Lenovo.Id
                    },
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }

    }
}
