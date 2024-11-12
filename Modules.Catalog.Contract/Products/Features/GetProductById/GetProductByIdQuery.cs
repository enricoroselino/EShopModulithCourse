using EShopModulithCourse.Server.Shared.CQRS;
using Modules.Catalog.Contract.Products.Dtos;

namespace Modules.Catalog.Contract.Products.Features.GetProductById;

public record GetProductByIdResult(ProductDto Product);

public record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResult>;