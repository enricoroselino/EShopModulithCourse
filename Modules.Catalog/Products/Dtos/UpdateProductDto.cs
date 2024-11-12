namespace Modules.Catalog.Products.Dtos;

public record UpdateProductDto(Guid Id, string Name, decimal Price);