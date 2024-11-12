using EShopModulithCourse.Server.Shared;
using EShopModulithCourse.Server.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Data.Seeders.Fakers;

namespace Modules.Catalog.Data.Seeders;

public class CatalogSeeder(CatalogDbContext catalogDbContext) : IDataSeeder
{
    public async Task SeedAsync()
    {
        await catalogDbContext.BeginTransaction(async () => { await AddProducts(10); });
    }

    private async Task AddProducts(int count)
    {
        if (await catalogDbContext.Products.AnyAsync()) return;

        var productFaker = new ProductFaker();
        var products = productFaker.Generate(count);
        await catalogDbContext.Products.AddRangeAsync(products);
        await catalogDbContext.SaveChangesAsync();
    }
}