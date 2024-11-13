using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Catalog.Products.Models;

namespace Modules.Catalog.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(150).IsRequired();
        builder.Property(c => c.Categories).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(255);
        builder.Property(c => c.ImageUrl).HasMaxLength(255);
        builder.Property(c => c.Price).HasColumnType("decimal(18, 4)").IsRequired();
    }
}