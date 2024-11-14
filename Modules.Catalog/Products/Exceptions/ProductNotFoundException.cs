using Shared.Exceptions;

namespace Modules.Catalog.Products.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid id)
        : base("Product", id)
    {
    }
}