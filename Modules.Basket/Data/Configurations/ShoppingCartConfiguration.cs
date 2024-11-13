using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Basket.Basket.Models;

namespace Modules.Basket.Data.Configurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();
        
        builder.HasIndex(e => e.UserName)
            .IsUnique();
        
        builder.Property(e => e.UserName)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(si => si.ShoppingCartId);
    }
}