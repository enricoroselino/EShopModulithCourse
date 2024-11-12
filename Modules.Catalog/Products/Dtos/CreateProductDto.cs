namespace Modules.Catalog.Products.Dtos;

public record CreateProductDto(string Name, List<string> Categories, string Description, string ImageUrl, decimal Price);