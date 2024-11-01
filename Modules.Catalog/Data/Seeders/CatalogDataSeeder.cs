using EShopModulithCourse.Server.Shared;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Data.Seeders.Fakers;

namespace Modules.Catalog.Data.Seeders;

public class CatalogDataSeeder : IDataSeeder
{
    private readonly CatalogDbContext _catalogDbContext;

    public CatalogDataSeeder(CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task SeedAsync()
    {
        await using var transaction = await _catalogDbContext.Database.BeginTransactionAsync();

        if (!await _catalogDbContext.Products.AnyAsync())
        {
            var products = new ProductFaker().Generate(10);
            await _catalogDbContext.Products.AddRangeAsync(products);
            await _catalogDbContext.SaveChangesAsync();
        }

        await transaction.CommitAsync();
    }
}