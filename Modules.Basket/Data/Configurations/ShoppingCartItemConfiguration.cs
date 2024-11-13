using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Basket.Basket.Models;

namespace Modules.Basket.Data.Configurations;

public class ShoppingCartItemConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(e => e.ProductId)
            .IsRequired();

        builder.Property(e => e.Quantity)
            .IsRequired();

        builder.Property(e => e.ProductName)
            .IsRequired();

        builder.Property(e => e.Price)
            .HasColumnType("decimal(18, 4)")
            .IsRequired();

        builder.Property(e => e.Color);
    }
}