using Shared.Contracts.DDD;

namespace Modules.Integration;

public record ProductPriceChangedIntegrationEvent : IIntegrationEvent
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public List<string> Category { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public decimal Price { get; set; }
}