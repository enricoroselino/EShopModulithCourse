using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Modules.Catalog.Features.Products.Models;

namespace Modules.Catalog.Data;

public class CatalogDbContext : DbContext
{
    private const string Schema = "catalog";
    private readonly IConfiguration _configuration;

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetValue<string>("APP_DB_CONNECTION"), SqlServerOptionsAction);
        base.OnConfiguring(optionsBuilder);
        return;

        void SqlServerOptionsAction(SqlServerDbContextOptionsBuilder o) =>
            o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}