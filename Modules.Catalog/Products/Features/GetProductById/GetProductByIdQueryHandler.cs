using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Contract.Products.Dtos;
using Modules.Catalog.Contract.Products.Features.GetProductById;
using Modules.Catalog.Data;
using Modules.Catalog.Products.Exceptions;
using Shared.Contracts.CQRS;

namespace Modules.Catalog.Products.Features.GetProductById;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    private readonly CatalogDbContext _catalogDbContext;

    public GetProductByIdQueryHandler(CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _catalogDbContext.Products
            .AsNoTracking()
            .Where(x => x.Id == request.ProductId)
            .SingleOrDefaultAsync(cancellationToken);

        if (product is null) throw new ProductNotFoundException(request.ProductId);

        var productDto = new ProductDto(
            product.Id,
            product.Name,
            product.Categories,
            product.Description,
            product.ImageUrl,
            product.Price
        );

        return new GetProductByIdResult(productDto);
    }
}