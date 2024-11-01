using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace EShopModulithCourse.Server.Shared.Extensions;

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

        var primarySeeders = seeders
            .Where(x => !x.IsDummyData)
            .Select(x => x.SeedAsync());

        var dummySeeders = seeders
            .Where(x => x.IsDummyData)
            .Select(x => x.SeedAsync());

        // wait until primary data seeded
        await Task.WhenAll(primarySeeders);
        // then seed the dummy data
        await Task.WhenAll(dummySeeders);
    }

    public static async Task<TResult> BeginTransaction<TResult>(this DbContext dbContext, Func<Task<TResult>> action)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var result = await action();
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw; // Re-throw the exception to avoid suppressing it
        }
    }

    public static async Task BeginTransaction(this DbContext dbContext, Func<Task> action)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            await action();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw; // Re-throw the exception to avoid suppressing it
        }
    }
}