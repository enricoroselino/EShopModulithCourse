using System.Reflection;
using EShopModulithCourse.Server.Shared.Interceptors;
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
    private readonly AuditableEntityInterceptor _auditableEntityInterceptor;

    public CatalogDbContext(
        DbContextOptions<CatalogDbContext> options,
        IConfiguration configuration,
        AuditableEntityInterceptor auditableEntityInterceptor) : base(options)
    {
        _configuration = configuration;
        _auditableEntityInterceptor = auditableEntityInterceptor;
    }

    public DbSet<Product> Products { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetValue<string>("APP_DB_CONNECTION"), SqlServerOptionsAction);
        optionsBuilder.AddInterceptors(_auditableEntityInterceptor);
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