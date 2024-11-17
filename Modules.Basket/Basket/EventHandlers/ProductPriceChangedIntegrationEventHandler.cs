using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Modules.Basket.Basket.Features.UpdateItemPriceInBasket;
using Modules.Integration;

namespace Modules.Basket.Basket.EventHandlers;

public class ProductPriceChangedIntegrationEventHandler : IConsumer<ProductPriceChangedIntegrationEvent>
{
    private readonly ILogger<ProductPriceChangedIntegrationEventHandler> _logger;
    private readonly ISender _mediator;

    public ProductPriceChangedIntegrationEventHandler(
        ILogger<ProductPriceChangedIntegrationEventHandler> logger,
        ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        _logger.LogInformation("Integration event handled: {IIntegrationEvent}", context.Message.GetType().Name);

        var command = new UpdateItemPriceInBasketCommand(context.Message.ProductId, context.Message.Price);
        var result = await _mediator.Send(command);
    }
}