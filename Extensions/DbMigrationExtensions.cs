using Microsoft.EntityFrameworkCore;
using StoreApp.Context;
using StoreApp.Products;
using StoreApp.Ingredients;

namespace StoreApp.Extensions;

public static class DbMigrationExtensions
{
    private const int RetryAttempts = 5;
    private const int RetryDelayMs = 2000;

    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

        // Make retries to apply migrations in case of transient errors or database not ready
        for (var i = 0; i < RetryAttempts; i++)
        {
            try
            {
                await context.Database.MigrateAsync();
                break;
            }
            catch (Exception) when (i < RetryAttempts - 1)
            {
                Thread.Sleep(RetryDelayMs);
            }
        }
    }

    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

        // Seed ingredients first
        if (!await context.Ingredients.AnyAsync())
        {
            var ingredients = new List<Ingredient>
            {
                new() { Name = "Chocolate", InStock = true },
                new() { Name = "Flour", InStock = true },
                new() { Name = "Sugar", InStock = true },
                new() { Name = "Eggs", InStock = true },
                new() { Name = "Butter", InStock = true },
                new() { Name = "Vanilla Extract", InStock = true },
                new() { Name = "Milk", InStock = false }, // One ingredient out of stock for testing
                new() { Name = "Strawberries", InStock = true },
                new() { Name = "Bananas", InStock = true },
                new() { Name = "Cream", InStock = true },
                new() { Name = "Ice", InStock = true },
                new() { Name = "Honey", InStock = true }
            };

            context.Ingredients.AddRange(ingredients);
            await context.SaveChangesAsync();
        }

        // Seed products with prices
        if (!await context.Products.AnyAsync())
        {
            var products = new List<Product>
            {
                new() { Name = "Chocolate Cake", Price = 25.99m },
                new() { Name = "Vanilla Ice Cream", Price = 5.99m },
                new() { Name = "Strawberry Smoothie", Price = 7.50m },
                new() { Name = "Fruit Salad", Price = 8.99m }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            // Now assign ingredients to products
            // Need to reload with tracking to assign relationships
            var chocolateCake = await context.Products
                .Include(p => p.Ingredients)
                .FirstAsync(p => p.Name == "Chocolate Cake");
            var vanillaIceCream = await context.Products
                .Include(p => p.Ingredients)
                .FirstAsync(p => p.Name == "Vanilla Ice Cream");
            var strawberrySmoothie = await context.Products
                .Include(p => p.Ingredients)
                .FirstAsync(p => p.Name == "Strawberry Smoothie");
            var fruitSalad = await context.Products
                .Include(p => p.Ingredients)
                .FirstAsync(p => p.Name == "Fruit Salad");

            // Get ingredients
            var chocolate = await context.Ingredients.FirstAsync(i => i.Name == "Chocolate");
            var flour = await context.Ingredients.FirstAsync(i => i.Name == "Flour");
            var sugar = await context.Ingredients.FirstAsync(i => i.Name == "Sugar");
            var eggs = await context.Ingredients.FirstAsync(i => i.Name == "Eggs");
            var butter = await context.Ingredients.FirstAsync(i => i.Name == "Butter");
            var vanillaExtract = await context.Ingredients.FirstAsync(i => i.Name == "Vanilla Extract");
            var milk = await context.Ingredients.FirstAsync(i => i.Name == "Milk");
            var strawberries = await context.Ingredients.FirstAsync(i => i.Name == "Strawberries");
            var bananas = await context.Ingredients.FirstAsync(i => i.Name == "Bananas");
            var cream = await context.Ingredients.FirstAsync(i => i.Name == "Cream");
            var ice = await context.Ingredients.FirstAsync(i => i.Name == "Ice");
            var honey = await context.Ingredients.FirstAsync(i => i.Name == "Honey");

            // Chocolate Cake ingredients
            chocolateCake.Ingredients.Add(chocolate);
            chocolateCake.Ingredients.Add(flour);
            chocolateCake.Ingredients.Add(sugar);
            chocolateCake.Ingredients.Add(eggs);
            chocolateCake.Ingredients.Add(butter);

            // Vanilla Ice Cream ingredients (includes milk which is out of stock)
            vanillaIceCream.Ingredients.Add(milk);
            vanillaIceCream.Ingredients.Add(cream);
            vanillaIceCream.Ingredients.Add(sugar);
            vanillaIceCream.Ingredients.Add(vanillaExtract);

            // Strawberry Smoothie ingredients
            strawberrySmoothie.Ingredients.Add(strawberries);
            strawberrySmoothie.Ingredients.Add(bananas);
            strawberrySmoothie.Ingredients.Add(ice);
            strawberrySmoothie.Ingredients.Add(honey);

            // Fruit Salad ingredients
            fruitSalad.Ingredients.Add(strawberries);
            fruitSalad.Ingredients.Add(bananas);
            fruitSalad.Ingredients.Add(honey);

            await context.SaveChangesAsync();
        }
    }
}
