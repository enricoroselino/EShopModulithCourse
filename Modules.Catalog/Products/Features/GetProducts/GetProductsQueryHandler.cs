using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Contract.Products.Dtos;
using Modules.Catalog.Data;
using Shared.Contracts.CQRS;
using Shared.Models;

namespace Modules.Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest Request) : IQuery<GetProductsResult>;

public record GetProductsResult(PaginatedResult<ProductDto> Products);

public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly CatalogDbContext _catalogDbContext;

    public GetProductsQueryHandler(CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.Request.PageIndex;
        var pageSize = query.Request.PageSize;
        var totalCount = await _catalogDbContext.Products.LongCountAsync(cancellationToken);

        var products = await _catalogDbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .Select(p => new ProductDto(p.Id, p.Name, p.Categories, p.Description, p.ImageUrl, p.Price))
            .ToListAsync(cancellationToken);

        var paginatedResult = new PaginatedResult<ProductDto>(pageIndex, pageSize, totalCount, products);
        return new GetProductsResult(paginatedResult);
    }
}