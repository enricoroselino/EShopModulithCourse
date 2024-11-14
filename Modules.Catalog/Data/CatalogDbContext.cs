using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Modules.Catalog.Products.Models;

namespace Modules.Catalog.Data;

public class CatalogDbContext : DbContext
{
    private const string Schema = "catalog";
    private readonly IConfiguration _configuration;

    public CatalogDbContext(
        DbContextOptions<CatalogDbContext> options,
        IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Product> Products { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetValue<string>("APP_DB_CONNECTION");
        
        optionsBuilder.UseSqlServer(connectionString, options =>
        {
            options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema);
        });
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}