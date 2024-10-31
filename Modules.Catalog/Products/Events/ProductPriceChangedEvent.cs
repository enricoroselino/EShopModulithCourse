using Modules.Catalog.Products.Models;

namespace Modules.Catalog.Products.Events;

public class ProductPriceChangedEvent(Product Product) : IDomainEvent;