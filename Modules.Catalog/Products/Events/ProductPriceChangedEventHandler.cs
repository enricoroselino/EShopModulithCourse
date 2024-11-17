using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Modules.Catalog.Products.Models;
using Modules.Integration;
using Shared.Contracts.DDD;

namespace Modules.Catalog.Products.Events;

public record ProductPriceChangedEvent(Product Product) : IDomainEvent;

public class ProductPriceChangedEventHandler : INotificationHandler<ProductPriceChangedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;
    private readonly IBus _bus;

    public ProductPriceChangedEventHandler(ILogger<ProductCreatedEventHandler> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event handled : {DomainEvent}", notification.GetType().Name);
        
        // publish product price changed integration event for update basket price
        var integrationEvent = new ProductPriceChangedIntegrationEvent
        {
            ProductId = notification.Product.Id,
            Category = notification.Product.Categories,
            Description = notification.Product.Description,
            ImageUrl = notification.Product.ImageUrl,
            Price = notification.Product.Price
        };

        await _bus.Publish(integrationEvent, cancellationToken);
    }
}