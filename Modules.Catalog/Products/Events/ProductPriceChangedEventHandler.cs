using MediatR;
using Microsoft.Extensions.Logging;
using Modules.Catalog.Products.Models;
using Shared.Contracts.DDD;

namespace Modules.Catalog.Products.Events;

public class ProductPriceChangedEvent(Product Product) : IDomainEvent;

public class ProductPriceChangedEventHandler : INotificationHandler<ProductPriceChangedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductPriceChangedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        // TODO: publish product price changed integration event for update basket price
        _logger.LogInformation("Domain event handled : {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}