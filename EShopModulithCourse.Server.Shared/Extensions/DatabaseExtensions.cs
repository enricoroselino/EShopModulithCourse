using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace EShopModulithCourse.Server.Shared.Extensions;

public static class DatabaseExtensions
{
    public static async Task MigrateDatabaseAsync<TDbContext>(this IApplicationBuilder app) where TDbContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();
    }

    public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        await Task.WhenAll(seeders.Select(x => x.SeedAsync()));
    }
}