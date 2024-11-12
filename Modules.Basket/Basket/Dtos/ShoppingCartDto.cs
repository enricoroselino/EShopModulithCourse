namespace Modules.Basket.Basket.Dtos;

public record ShoppingCartDto(Guid Id, List<ShoppingCartItemDto> Items);