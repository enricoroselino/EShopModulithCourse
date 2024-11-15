using Modules.Catalog.Products.Events;
using Shared.Contracts.DDD;

namespace Modules.Catalog.Products.Models;

public class Product : Aggregate<Guid>
{
    private Product()
    {
    }

    public string Name { get; private set; } = default!;
    public List<string> Categories { get; private set; } = new();
    public string Description { get; private set; } = default!;
    public string ImageUrl { get; private set; } = default!;
    public decimal Price { get; private set; }

    public static Product Create(string name, string description, List<string> categories, decimal price,
        string imageUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Categories = categories,
            Price = price,
            ImageUrl = imageUrl
        };

        product.AddDomainEvent(new ProductCreatedEvent(product));
        return product;
    }

    public void Update(string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        Name = name;

        if (Price != price)
        {
            Price = price;
            AddDomainEvent(new ProductPriceChangedEvent(this));
        }
    }
}