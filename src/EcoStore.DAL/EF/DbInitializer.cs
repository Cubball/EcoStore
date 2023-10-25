using EcoStore.DAL.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcoStore.DAL.EF;

public static class DbInitializer
{
    private static List<Brand> s_brands = default!;
    private static List<Category> s_categories = default!;
    private static List<Product> s_products = default!;
    private static List<AppUser> s_users = default!;
    private static List<Order> s_orders = default!;
    private static List<OrderedProduct> s_orderedProducts = default!;

    public static async Task Initialize(IServiceScope serviceScope)
    {
        var serviceProvider = serviceScope.ServiceProvider;
        using var context = serviceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();

        await AddAdminIfNotExists(serviceProvider);
        if (ShouldSeedDb(context))
        {
            await AddBrands(context);
            await AddCategories(context);
            await AddProducts(context);
            await AddUsers(serviceProvider.GetRequiredService<UserManager<AppUser>>());
            await AddOrders(context);
            await AddOrderedProducts(context);
        }
    }

    private static async Task AddAdminIfNotExists(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var adminRoleName = Role.Admin.ToString();
        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRoleName));
        }

        var adminRole = await roleManager.FindByNameAsync(adminRoleName);
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        if (await context.UserRoles.AnyAsync(ur => ur.RoleId == adminRole!.Id))
        {
            return;
        }

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var adminEmail = configuration["Admin:Email"];
        var adminPassword = configuration["Admin:Password"]!;
        var admin = new AppUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "admin",
            LastName = string.Empty,
        };

        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var result = await userManager.CreateAsync(admin, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, adminRoleName);
        }
    }

    private static async Task AddBrands(AppDbContext context)
    {
        s_brands = new List<Brand>()
        {
            new Brand
            {
                Name = "ЕкоВироби",
                Description = "ЕкоВироби - це бренд, що пропонує екологічні товари для дому та саду",
            },
            new Brand
            {
                Name = "БіоГармонія",
                Description = "БіоГармонія спеціалізується на виробництві екологічних виробів, які виготовляються з натуральних матеріалів",
            },
            new Brand
            {
                Name = "Зелена Галявина",
                Description = "Зелена Галявина - відомий бренд, заснований у 2000 році, що відомий своїми еко-товарами для дому та саду",
            },
        };

        await context.Brands.AddRangeAsync(s_brands);
        await context.SaveChangesAsync();
    }

    private static async Task AddCategories(AppDbContext context)
    {
        s_categories = new List<Category>()
        {
            new Category
            {
                Name = "Для дому",
                Description = "Eco-friendly товари для дому",
            },
            new Category
            {
                Name = "Одяг та аксесуари",
                Description = "Одяг та аксесуари зроблені із екологічних матеріалів",
            },
            new Category
            {
                Name = "Засоби гігієни",
                Description = "Засоби гігєни, виготовлені з натуральних інгредієнтів не лише допомагають природі, але й кращі для здоров'я",
            },
        };
        context.Categories.AddRange(s_categories);
        await context.SaveChangesAsync();
    }

    private static async Task AddProducts(AppDbContext context)
    {
        s_products = new List<Product>()
        {
            new Product
            {
                Name = "Еко мило",
                Description = "Екологічне мило зроблене з натуральних інгредієнтів",
                Price = 100,
                ImageUrl = "ecostore.com/images/soap.jpg",
                Stock = 100,
                CategoryId = s_categories[2].Id,
                BrandId = s_brands[0].Id,
            },
            new Product
            {
                Name = "Ремінь",
                Description = "Ремінь зроблений з еко-шкіри",
                Price = 500,
                ImageUrl = "ecostore.com/images/belt.jpg",
                Stock = 50,
                CategoryId = s_categories[1].Id,
                BrandId = s_brands[0].Id,
            },
            new Product
            {
                Name = "Еко-шоппер",
                Description = "Екологічна сумка для покупок",
                Price = 200,
                ImageUrl = "ecostore.com/images/bag.jpg",
                Stock = 200,
                CategoryId = s_categories[1].Id,
                BrandId = s_brands[2].Id,
            },
            new Product
            {
                Name = "Набір дерев'яних ложок",
                Description = "Набір дерев'яних ложок для страв",
                Price = 300,
                ImageUrl = "ecostore.com/images/spoons.jpg",
                Stock = 150,
                CategoryId = s_categories[0].Id,
                BrandId = s_brands[2].Id,
            },
            new Product
            {
                Name = "Біо-розкладні пакети",
                Description = "Біорозкладні пакети для сміття",
                Price = 50,
                ImageUrl = "ecostore.com/images/bags.jpg",
                Stock = 500,
                CategoryId = s_categories[0].Id,
                BrandId = s_brands[1].Id,
            },
            new Product
            {
                Name = "Еко шампунь",
                Description = "Екологічний шампунь зроблений з натуральних інгредієнтів",
                Price = 150,
                ImageUrl = "ecostore.com/images/shampoo.jpg",
                Stock = 80,
                CategoryId = s_categories[2].Id,
                BrandId = s_brands[1].Id,
            },
            new Product
            {
                Name = "Свічка",
                Description = "Екологічна свічка зроблена з натуральних інгредієнтів",
                Price = 100,
                ImageUrl = "ecostore.com/images/candle.jpg",
                Stock = 100,
                CategoryId = s_categories[0].Id,
                BrandId = s_brands[2].Id,
            },
            new Product
            {
                Name = "Зубна паста",
                Description = "Екологічна зубна паста зроблена з натуральних інгредієнтів",
                Price = 100,
                ImageUrl = "ecostore.com/images/toothpaste.jpg",
                Stock = 100,
                CategoryId = s_categories[2].Id,
                BrandId = s_brands[1].Id,
            },
            new Product
            {
                Name = "Засіб для миття посуду",
                Description = "Екологічний засіб для миття посуду зроблений з натуральних інгредієнтів",
                Price = 100,
                ImageUrl = "ecostore.com/images/dishwashing.jpg",
                Stock = 100,
                CategoryId = s_categories[2].Id,
                BrandId = s_brands[0].Id,
            },
            new Product
            {
                Name = "Пляшка для води",
                Description = "Скляна пляшка для води, яку можна використовувати повторно",
                Price = 200,
                ImageUrl = "ecostore.com/images/bottle.jpg",
                Stock = 200,
                CategoryId = s_categories[0].Id,
                BrandId = s_brands[2].Id,
            }
        };
        context.Products.AddRange(s_products);
        await context.SaveChangesAsync();
    }

    private static async Task AddUsers(UserManager<AppUser> userManager)
    {
        s_users = new List<AppUser>
        {
            new AppUser
            {
                UserName = "alex@gmail.com",
                Email = "alex@gmail.com",
                FirstName = "Олександр",
                LastName = "Шевчук",
            },
            new AppUser
            {
                UserName = "andrew123@ukr.net",
                Email = "andrew123@ukr.net",
                FirstName = "Андрій",
                LastName = "Ковальчук",
            },
            new AppUser
            {
                UserName = "ira_a@gmail.com",
                Email = "ira_a@gmail.com",
                FirstName = "Ірина",
                LastName = "Андріїв",
            },
        };
        foreach (var user in s_users)
        {
            await userManager.CreateAsync(user, "password");
        }
    }

    private static async Task AddOrders(AppDbContext context)
    {
        s_orders = new List<Order>
        {
            new Order
            {
                UserId = s_users[0].Id,
                OrderDate = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                OrderStatus = OrderStatus.Completed,
                StatusChangedDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                PaymentMethod = PaymentMethod.Cash,
                PaymentId = null,
                ShippingAddress = "вул. Шевченка, 1, м. Львів, 79000",
                ShippingMethod = ShippingMethod.NovaPoshta,
                TrackingNumber = "1234567890",
            },
            new Order
            {
                UserId = s_users[1].Id,
                OrderDate = DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                OrderStatus = OrderStatus.Delivered,
                StatusChangedDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                PaymentMethod = PaymentMethod.Cash,
                PaymentId = null,
                ShippingAddress = "вул. Шевченка 2, м. Київ, 02000",
                ShippingMethod = ShippingMethod.UkrPoshta,
                TrackingNumber = "1234567890",
            },
            new Order
            {
                UserId = s_users[0].Id,
                OrderDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                OrderStatus = OrderStatus.Processing,
                StatusChangedDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                PaymentMethod = PaymentMethod.Cash,
                PaymentId = null,
                ShippingAddress = "вул. Шевченка, 1, м. Львів, 79000",
                ShippingMethod = ShippingMethod.NovaPoshta,
                TrackingNumber = "1234567890",
            },
            new Order
            {
                UserId = s_users[2].Id,
                OrderDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                OrderStatus = OrderStatus.CancelledByUser,
                StatusChangedDate = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
                PaymentMethod = PaymentMethod.Cash,
                ShippingAddress = "вул. Степана Бандери 6, м. Київ, 02000",
                PaymentId = null,
                ShippingMethod = ShippingMethod.NovaPoshta,
                TrackingNumber = "1234567890",
            },
            new Order
            {
                UserId = s_users[1].Id,
                OrderDate = DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                OrderStatus = OrderStatus.Delivering,
                StatusChangedDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                PaymentMethod = PaymentMethod.Cash,
                ShippingAddress = "вул. Володимира Сосюри 52, м. Дубно 35600",
                PaymentId = null,
                ShippingMethod = ShippingMethod.NovaPoshta,
                TrackingNumber = "1234567890",
            },
        };
        context.Orders.AddRange(s_orders);
        await context.SaveChangesAsync();
    }

    private static async Task AddOrderedProducts(AppDbContext context)
    {
        s_orderedProducts = new List<OrderedProduct>
        {
            new OrderedProduct
            {
                OrderId = s_orders[0].Id,
                ProductId = s_products[0].Id,
                Quantity = 2,
                ProductPrice = s_products[0].Price,
            },
            new OrderedProduct
            {
                OrderId = s_orders[0].Id,
                ProductId = s_products[1].Id,
                Quantity = 1,
                ProductPrice = s_products[1].Price,
            },
            new OrderedProduct
            {
                OrderId = s_orders[1].Id,
                ProductId = s_products[2].Id,
                Quantity = 1,
                ProductPrice = s_products[2].Price,
            },
            new OrderedProduct
            {
                OrderId = s_orders[2].Id,
                ProductId = s_products[3].Id,
                Quantity = 3,
                ProductPrice = s_products[3].Price,
            },
            new OrderedProduct
            {
                OrderId = s_orders[2].Id,
                ProductId = s_products[4].Id,
                Quantity = 10,
                ProductPrice = s_products[4].Price,
            },
            new OrderedProduct
            {
                OrderId = s_orders[2].Id,
                ProductId = s_products[5].Id,
                Quantity = 1,
                ProductPrice = s_products[5].Price,
            },
            new OrderedProduct
            {
                OrderId = s_orders[3].Id,
                ProductId = s_products[6].Id,
                Quantity = 1,
                ProductPrice = s_products[6].Price,
            },
            new OrderedProduct
            {
                OrderId = s_orders[4].Id,
                ProductId = s_products[7].Id,
                Quantity = 1,
                ProductPrice = s_products[7].Price,
            },
        };
        context.OrderedProducts.AddRange(s_orderedProducts);
        await context.SaveChangesAsync();
    }

    private static bool ShouldSeedDb(AppDbContext context)
    {
        return !context.Products.Any() &&
               !context.Categories.Any() &&
               !context.Brands.Any() &&
               !context.Orders.Any() &&
               !context.OrderedProducts.Any();
    }
}