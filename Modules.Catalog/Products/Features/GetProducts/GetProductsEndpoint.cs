using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Catalog.Contract.Products.Dtos;
using Shared;

namespace Modules.Catalog.Products.Features.GetProducts;

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery(request));

                return Results.Ok(result.Products);
            })
            .WithName("GetProducts")
            .Produces<PaginatedResult<ProductDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
    }
}