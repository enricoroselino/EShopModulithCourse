using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Catalog.Data.Seeders.Fakers;
using Shared.Interfaces;

namespace Modules.Catalog.Data.Seeders;

public class CatalogSeeder(CatalogDbContext catalogDbContext, ILogger<CatalogSeeder> logger) : IDataSeeder
{
    public async Task SeedAsync()
    {
        await using var transaction = await catalogDbContext.Database.BeginTransactionAsync();
        try
        {
            await AddProducts();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            await transaction.RollbackAsync();
        }
    }

    private async Task AddProducts()
    {
        if (await catalogDbContext.Products.AnyAsync()) return;

        var productFaker = new ProductFaker();
        var products = productFaker.Generate(10);
        await catalogDbContext.Products.AddRangeAsync(products);
        await catalogDbContext.SaveChangesAsync();
    }
}