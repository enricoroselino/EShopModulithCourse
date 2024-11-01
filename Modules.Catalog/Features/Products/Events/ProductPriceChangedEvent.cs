using Modules.Catalog.Features.Products.Models;

namespace Modules.Catalog.Features.Products.Events;

public class ProductPriceChangedEvent(Product Product) : IDomainEvent;