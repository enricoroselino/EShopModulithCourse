using Modules.Catalog.Contract.Products.Dtos;
using Shared.Contracts.CQRS;

namespace Modules.Catalog.Contract.Products.Features.GetProductById;

public record GetProductByIdResult(ProductDto Product);

public record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResult>;