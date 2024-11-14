using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Data;

public static class DatabaseExtensions
{
    public static async Task MigrateDatabaseAsync<TDbContext>(this IApplicationBuilder app) where TDbContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
        await context.Database.MigrateAsync();
    }

    public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var seeders = scope.ServiceProvider
            .GetServices<IDataSeeder>()
            .ToList();

        var seeder = seeders.Select(x => x.SeedAsync());
        await Task.WhenAll(seeder);
    }
}