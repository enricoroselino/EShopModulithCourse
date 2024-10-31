using Modules.Catalog.Products.Models;

namespace Modules.Catalog.Products.Events;

public record ProductCreatedEvent(Product Product) : IDomainEvent;
