using Modules.Catalog.Features.Products.Models;

namespace Modules.Catalog.Features.Products.Events;

public record ProductCreatedEvent(Product Product) : IDomainEvent;
