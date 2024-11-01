using Bogus;
using EShopModulithCourse.Server.Shared.Providers;
using Modules.Catalog.Features.Products.Models;

namespace Modules.Catalog.Data.Seeders.Fakers;

internal sealed class ProductFaker : Faker<Product>
{
    public ProductFaker()
    {
        RuleFor(x => x.Id, _ => UuidProvider.NewSequential());
        RuleFor(x => x.Name, f => f.Commerce.ProductName());
        RuleFor(x => x.Description, f => f.Commerce.ProductDescription());
        RuleFor(x => x.Price, f => int.Parse(f.Commerce.Price()));
        RuleFor(x => x.Categories, f => f.Commerce.Categories(3).ToList());
        RuleFor(x => x.ImageUrl, f => f.Internet.Avatar());
    }
}