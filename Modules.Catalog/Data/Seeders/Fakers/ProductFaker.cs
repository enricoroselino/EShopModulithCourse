using Bogus;
using Modules.Catalog.Features.Products.Models;

namespace Modules.Catalog.Data.Seeders.Fakers;

internal sealed class ProductFaker : Faker<Product>
{
    public ProductFaker()
    {
        CustomInstantiator(f => Product.Create(
            name: f.Commerce.ProductName(),
            description: f.Commerce.ProductDescription(),
            categories: f.Commerce.Categories(new Random().Next(1, 5)).ToList(),
            price: decimal.Parse(f.Commerce.Price()),
            imageUrl: f.Image.PicsumUrl())
        );
    }
}