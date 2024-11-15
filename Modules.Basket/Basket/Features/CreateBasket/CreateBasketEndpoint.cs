using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Basket.Basket.Dtos;

namespace Modules.Basket.Basket.Features.CreateBasket;

public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket",
                async (CreateShoppingCartDto request, ISender sender) =>
                {
                    var command = new CreateBasketCommand(request);
                    var result = await sender.Send(command);
                    return Results.Created($"/basket/{result.ShoppingCartId}", result);
                })
            .Produces<CreateBasketResult>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Basket")
            .WithDescription("Create Basket");
        //.RequireAuthorization();
    }
}