using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Catalog.Products.Dtos;

namespace Modules.Catalog.Products.Features.CreateProduct;

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductDto request, ISender sender) =>
            {
                var command = new CreateProductCommand(request);
                var result = await sender.Send(command);
                return Results.Created($"/products/{result.Id}", result);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResult>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
    }
}