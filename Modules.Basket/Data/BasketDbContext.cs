using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Modules.Basket.Basket.Models;

namespace Modules.Basket.Data;

public class BasketDbContext : DbContext
{
    private const string Schema = "basket";
    private readonly IConfiguration _configuration;

    public BasketDbContext(
        DbContextOptions<BasketDbContext> options,
        IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<ShoppingCart> ShoppingCarts { get; init; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetValue<string>("APP_DB_CONNECTION");

        optionsBuilder.UseSqlServer(connectionString,
            options => { options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema); });
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}