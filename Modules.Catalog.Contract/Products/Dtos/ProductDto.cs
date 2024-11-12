namespace Modules.Catalog.Contract.Products.Dtos;

public record ProductDto(Guid Id, string Name, List<string> Categories, string Description, string ImageUrl, decimal Price);