using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Modules.Basket.Basket.Dtos;

namespace Modules.Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketRequest(Guid ProductId, int Quantity, string Color);

public class AddItemIntoBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{userName}/items",
                async (
                    [FromRoute] string userName,
                    [FromBody] AddItemIntoBasketRequest request,
                    ISender sender) =>
                {
                    var command = new AddItemIntoBasketCommand(userName, request);
                    var result = await sender.Send(command);

                    return Results.Created($"/basket/{result.ShoppingCartId}", result);
                })
            .Produces<AddItemIntoBasketResult>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add Item Into Basket")
            .WithDescription("Add Item Into Basket");
        //.RequireAuthorization();
    }
}