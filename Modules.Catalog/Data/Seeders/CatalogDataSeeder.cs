using EShopModulithCourse.Server.Shared;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Data.Seeders.Fakers;

namespace Modules.Catalog.Data.Seeders;

public class CatalogDataSeeder(CatalogDbContext catalogDbContext) : IDataSeeder
{
    public async Task SeedAsync()
    {
        if (!await catalogDbContext.Products.AnyAsync())
        {
            var products = new ProductFaker().Generate(10);
            await catalogDbContext.Products.AddRangeAsync(products);
            await catalogDbContext.SaveChangesAsync();
        }
    }
}