using MediatR;
using Microsoft.Extensions.Logging;
using Modules.Catalog.Products.Models;

namespace Modules.Catalog.Products.Events;

public record ProductCreatedEvent(Product Product) : IDomainEvent;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event handled : {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}